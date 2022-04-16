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
    }
}
