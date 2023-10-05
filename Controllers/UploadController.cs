using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : BaseController
    {
        private IConfiguration configuration;
        private readonly ILogger<UploadController> _logger;
        private readonly ResponseContext _responseData;
        public int chunkSize;
        private string tempFolder;
        private string destinationFolder;

        public UploadController(IConfiguration configuration, ILogger<UploadController> logger)
        {
            this.configuration = configuration;
            _logger = logger;
            chunkSize = 1048576 * Convert.ToInt32(configuration["ChunkSize"]);
            tempFolder = configuration["TempFolder"];
            destinationFolder = configuration["TargetFolder"];
            _responseData = new ResponseContext();
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
            }
            catch (Exception ex)
            {
                _responseData.ErrorMessage = ex.Message;
                _responseData.IsSuccess = false;
            }
            return Ok(_responseData);
        }

        [HttpPost("UploadComplete")]
        public IActionResult UploadComplete(string fileName)
        {
            try
            {
                FileInfo fi = new FileInfo(fileName);
                string name = fi.Name.Replace(fi.Extension, string.Empty);
                string newPath = $"{destinationFolder}/{fileName}";

                string[] filePaths = Directory.GetFiles(tempFolder).Where(p => p.Contains(name)).OrderBy(p => p.Split('_')[1]).ToArray();
                foreach (string filePath in filePaths)
                {
                    MergeChunks(newPath, filePath);
                }
            }
            catch (Exception ex)
            {
                _responseData.ErrorMessage = ex.Message;
                _responseData.IsSuccess = false;
            }
            return Ok(_responseData);
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
