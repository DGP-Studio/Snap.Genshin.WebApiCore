using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.Genshin.WebApi.Entities
{
    public class MetadataInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; } = null!;

        [Required]
        public string ContentHash { get; set; } = null!;
    }
}
