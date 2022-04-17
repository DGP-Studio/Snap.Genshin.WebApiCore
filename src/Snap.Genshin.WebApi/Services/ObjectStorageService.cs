using Snap.Genshin.WebApi.Entities;
using Snap.Genshin.WebApi.Utilities;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Snap.Genshin.WebApi.Services
{
    public class ObjectStorageService
    {
        public ObjectStorageService(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.MetaObjectPath = configuration.GetSection("Vfs").GetValue<string>("Metadata");

            if (!Directory.Exists(MetaObjectPath)) throw new Exception("Metadata路径不存在");
        }

        private readonly ApplicationDbContext dbContext;

        private readonly string MetaObjectPath = "";

        public async Task<(ApiCode Code, string Message)> UpdateFile(IFormFile file)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            var hash = HashStream(stream);
            var fileName = file.Name;

            var fileQuery = dbContext.MetadataInfo.Where(info => info.FileName == fileName);
            var metaInfo = fileQuery.Any() ? 
                               fileQuery.Single() : 
                               dbContext.MetadataInfo.Add(new MetadataInfo { FileName = fileName }).Entity;

            var oldHash = metaInfo.ContentHash;
            metaInfo.ContentHash = hash;

            try
            {
                // 删除旧文件（若存在）
                if (!string.IsNullOrEmpty(oldHash))
                {
                    File.Delete(Path.Combine(MetaObjectPath, fileName));
                }
                // 写入新文件
                using var vfsFile = File.OpenWrite(Path.Combine(MetaObjectPath, hash));
                await stream.CopyToAsync(vfsFile);
                await vfsFile.FlushAsync();
                vfsFile.Close();
            }
            catch (Exception ex)
            {
                return (ApiCode.Fail, ex.Message);
            }

            await dbContext.SaveChangesAsync();
            return (ApiCode.Success, "更新成功");
        }

        private static string HashStream(Stream stream)
        {
            using var sha256 = SHA256.Create();
            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            byte[] hash = sha256.ComputeHash(buffer);
            return ByteArrayToString(hash);
        }

        private static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
    }
}
