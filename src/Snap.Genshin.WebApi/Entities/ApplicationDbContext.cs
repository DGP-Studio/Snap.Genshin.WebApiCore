using Microsoft.EntityFrameworkCore;

namespace Snap.Genshin.WebApi.Entities
{
    ///<inheritdoc/>
    public class ApplicationDbContext : DbContext
    {
        ///<inheritdoc/>
        public ApplicationDbContext(DbContextOptions opt) : base(opt) { }

        /// <summary>
        /// 键值配置存储
        /// </summary>
        public DbSet<KeyValueConfig> KeyValueConfigs { get; set; } = null!;

        /// <summary>
        /// 元数据信息
        /// </summary>
        public DbSet<MetadataInfo> MetadataInfo { get; set; } = null!;
    }
}
