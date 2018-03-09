﻿using System;
using Xunit;

namespace WeihanLi.Redis.UnitTest
{
    public class CommonTest : BaseUnitTest
    {
        [Fact]
        public void CacheTest()
        {
            var key = "commonCacheTest11";
            var commonClient = RedisManager.GetCommonRedisClient(RedisDataType.Cache);
            var cacheClient = RedisManager.GetCacheClient();
            var ttl = commonClient.KeyTimeToLive(key);
            Assert.Null(ttl);
            Assert.False(commonClient.KeyExists(key));
            Assert.True(cacheClient.Set(key, Guid.NewGuid()));
            ttl = commonClient.KeyTimeToLive(key);
            Assert.Null(ttl);
            Assert.True(commonClient.KeyExists(key));
            commonClient.KeyExpireAsync(key, DateTime.Now.AddMinutes(2));
            Assert.NotNull(commonClient.KeyTimeToLive(key));
            Assert.True(commonClient.KeyDelete(key));
            Assert.False(cacheClient.Exists(key));
        }

        [Fact]
        public void CounterTest()
        {
            var key = "commonCounterTest11";
            var commonClient = RedisManager.GetCommonRedisClient(RedisDataType.Counter);

            Assert.False(commonClient.KeyExists(key));
            var cacheClient = RedisManager.GetCounterClient(key);
            var ttl = commonClient.KeyTimeToLive(key);
            Assert.Null(ttl);
            Assert.True(cacheClient.Increase() == 1);
            Assert.True(commonClient.KeyExists(key));
            commonClient.KeyExpireAsync(key, DateTime.Now.AddMinutes(2));
            Assert.NotNull(commonClient.KeyTimeToLive(key));
            Assert.True(commonClient.KeyDelete(key));
            Assert.False(commonClient.KeyExists(key));
        }
    }
}
