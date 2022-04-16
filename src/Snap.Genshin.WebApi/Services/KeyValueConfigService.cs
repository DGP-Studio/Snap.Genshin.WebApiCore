using Microsoft.Extensions.Caching.Distributed;
using Snap.Genshin.WebApi.Entities;

namespace Snap.Genshin.WebApi.Services
{
    /// <summary>
    /// 提供向数据库中存储/读取键值配置的服务
    /// </summary>
    public class KeyValueConfigService
    {
        /// <inheritdoc/>
        public KeyValueConfigService(ApplicationDbContext dbContext, IDistributedCache cache)
        {
            this.dbContext = dbContext;
            this.cache = cache;
        }

        /// <summary>
        /// 通过键获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetString(string key)
        {
            string value = cache.GetString(key);
            if (string.IsNullOrEmpty(value))
            {
                value = dbContext.KeyValueConfigs.SingleOrDefault(item => item.Key == key)?.Value ?? string.Empty;
                cache.SetString(key, value);
            }

            return value;
        }

        /// <summary>
        /// 设置键值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetString(string key, string value)
        {
            var storedKeyValuePairQuery = dbContext.KeyValueConfigs.Where(item => item.Key == key);
            var item = storedKeyValuePairQuery.Any() ? storedKeyValuePairQuery.Single() : dbContext.KeyValueConfigs.Add(new() { Key = key }).Entity;

            // 更新数据库
            item.Value = value;
            dbContext.SaveChanges();

            // 更新缓存
            cache.SetString(key, item.Value);
        }

        private readonly ApplicationDbContext dbContext;
        private readonly IDistributedCache cache;
    }
}
