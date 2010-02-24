﻿using MbCache.Configuration;
using MbCache.Core;
using MbCacheTest.CacheForTest;
using MbCacheTest.TestData;
using NUnit.Framework;

namespace MbCacheTest.Logic
{
    [TestFixture]
    public class InvalidateCacheTest
    {
        private IMbCacheFactory factory;

        [SetUp]
        public void Setup()
        {
            var builder = new CacheBuilder();
            builder.UseCacheForClass<ObjectReturningRandomNumbers>(c => c.CachedMethod());
            builder.UseCacheForClass<ObjectReturningRandomNumbers>(c => c.CachedMethod2());

            factory = builder.BuildFactory(new TestCacheFactory());
        }

        [Test]
        public void VerifyInvalidate()
        {
            var obj = factory.Create<ObjectReturningRandomNumbers>();
            var value1 = obj.CachedMethod();
            var value2 = obj.CachedMethod2();
            Assert.AreEqual(value1, obj.CachedMethod());
            Assert.AreEqual(value2, obj.CachedMethod2());
            Assert.AreNotEqual(value1, value2);
            factory.Invalidate<ObjectReturningRandomNumbers>();
            Assert.AreNotEqual(value1, obj.CachedMethod());
            Assert.AreNotEqual(value2, obj.CachedMethod2());
        }

        [Test]
        public void VerifyInvalidateSpecificMethod()
        {
            var obj = factory.Create<ObjectReturningRandomNumbers>();
            var value1 = obj.CachedMethod();
            var value2 = obj.CachedMethod2();
            factory.Invalidate<ObjectReturningRandomNumbers>(c => c.CachedMethod());
            Assert.AreNotEqual(value1, obj.CachedMethod());
            Assert.AreEqual(value2, obj.CachedMethod2());
        }
    }
}