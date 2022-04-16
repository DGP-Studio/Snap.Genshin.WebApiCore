using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.Genshin.WebApi.Entities
{
    /// <summary>
    /// 键值配置
    /// </summary>
    public class KeyValueConfig
    {
        /// <summary>
        /// Id
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int Id { get; set; }

        /// <summary>
        /// 配置的键名
        /// </summary>
        [Required]
        public string Key { get; set; } = null!;

        /// <summary>
        /// 配置值
        /// </summary>
        public string Value { get; set; } = null!;
    }
}
