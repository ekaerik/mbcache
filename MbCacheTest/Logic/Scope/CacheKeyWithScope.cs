﻿using MbCache.Configuration;

namespace MbCacheTest.Logic.CacheKeyPerComponent
{
	public class CacheKeyWithScope : CacheKeyBase
	{
		protected override string ParameterValue(object parameter)
		{
			return parameter.ToString();
		}

		public static string CurrentScope { get; set; }

		protected override string KeyStart()
		{
			return CurrentScope;
		}
	}
}