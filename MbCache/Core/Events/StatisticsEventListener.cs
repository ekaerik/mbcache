﻿using System.Threading;

namespace MbCache.Core.Events
{
	public class StatisticsEventListener : IEventListener
	{
		private long _cacheHits;
		private long _cacheMisses;

		void IEventListener.OnGet(CachedItem cachedItem, bool successful)
		{
			if (successful)
			{
				Interlocked.Increment(ref _cacheHits);
			}
			else
			{
				Interlocked.Increment(ref _cacheMisses);
			}
		}

		void IEventListener.OnDelete(CachedItem cachedItem)
		{
		}

		void IEventListener.OnPut(CachedItem cachedItem)
		{
		}

		public long CacheHits
		{
			get { return _cacheHits; }
		}

		public long CacheMisses
		{
			get { return _cacheMisses; }
		}

		public void Clear()
		{
			_cacheHits = 0;
			_cacheMisses = 0;
		}
	}
}