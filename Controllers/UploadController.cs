using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : BaseController<UploadController>
    {
        public int chunkSize;

        public UploadController(IConfiguration config, ILogger<UploadController> log) : base(config, log)
        {
            chunkSize = 1048576 * Convert.ToInt32(configuration["ChunkSize"]);
            targetFolder = $"{targetFolder}/Data";
        }

        [HttpPost("UploadChunks")]
        public async Task<IActionResult> UploadChunks(string id, string fileName)
        {
            try
            {
                var chunkNumber = id;
                FileInfo fi = new FileInfo(fileName);
                string newpath = $"{tempFolder}/{fi.Name.Replace(fi.Extension,string.Empty)}_{chunkNumber}{fi.Extension}";
                using (FileStream fs = System.IO.File.Create(newpath))
                {
                    byte[] bytes = new byte[chunkSize];
                    int bytesRead = 0;
                    while ((bytesRead = await Request.Body.ReadAsync(bytes, 0, bytes.Length)) > 0)
                    {
                        fs.Write(bytes, 0, bytesRead);
                    }
                }
                SetResponse(true, "Chunk uploaded successfully", null);
            }
            catch (Exception ex)
            {
                SetResponse(false, null, ex.Message);
            }
            return Ok(responseData);
        }

        [HttpPost("UploadComplete")]
        public IActionResult UploadComplete(string fileName)
        {
            try
            {
                FileInfo fi = new FileInfo(fileName);
                string name = fi.Name.Replace(fi.Extension, string.Empty);
                string newPath = $"{targetFolder}/{fileName}";

                string[] filePaths = Directory.GetFiles(tempFolder).Where(p => p.Contains(name)).OrderBy(p => p.Split('_')[1]).ToArray();
                foreach (string filePath in filePaths)
                {
                    MergeChunks(newPath, filePath);
                }
                SetResponse(true, "File uploaded successfully", null);
            }
            catch (Exception ex)
            {
                SetResponse(false, null, ex.Message);
            }
            return Ok(responseData);
        }

        private static void MergeChunks(string chunk1, string chunk2)
        {
            FileStream? fs1 = null;
            FileStream? fs2 = null;
            try
            {
                fs1 = System.IO.File.Open(chunk1, FileMode.Append);
                fs2 = System.IO.File.Open(chunk2, FileMode.Open);
                byte[] fs2Content = new byte[fs2.Length];
                fs2.Read(fs2Content, 0, (int)fs2.Length);
                fs1.Write(fs2Content, 0, (int)fs2.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " : " + ex.StackTrace);
            }
            finally
            {
                if (fs1 != null) fs1.Close();
                if (fs2 != null) fs2.Close();
                System.IO.File.Delete(chunk2);
            }
        }
    }

    
}
