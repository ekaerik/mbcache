﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text.RegularExpressions;

namespace MbCache.Configuration
{
	[Serializable]
	public class InMemoryCache : ICache
	{
		private readonly int _timeoutMinutes;
		private static readonly MemoryCache _cache = MemoryCache.Default;
		private static readonly Regex findSeperator = new Regex(@"\|", RegexOptions.Compiled);
		private static readonly object dependencyValue = new object();

		public InMemoryCache(int timeoutMinutes)
		{
			_timeoutMinutes = timeoutMinutes;
		}

		public object Get(string key)
		{
			return _cache.Get(key);
		}

		public void Put(string key, object value)
		{
			var keys = new List<string>();
			var matches = findSeperator.Matches(key);
			keys.AddRange(from Match match in matches select key.Substring(0, match.Index));

			createDependencies(keys);

			var policy = new CacheItemPolicy
			             	{
			             		AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(_timeoutMinutes)
			             	};
			if (keys.Count > 0)
			{
				policy.ChangeMonitors.Add(_cache.CreateCacheEntryChangeMonitor(keys));				
			}

			_cache.Set(key, value, policy);
		}

		private static void createDependencies(IEnumerable<string> keys)
		{
			foreach (var key in keys)
			{
				var policy = new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable };
				_cache.Add(key, dependencyValue, policy);
			}
		}

		public void Delete(string keyStartingWith)
		{
			var value = _cache.Get(keyStartingWith);
			if (value == null || value == dependencyValue)
			{
				var policy = new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable };
				_cache.Set(keyStartingWith, dependencyValue, policy);				
			}
			else
			{
				_cache.Remove(keyStartingWith);
			}
		}
	}
}