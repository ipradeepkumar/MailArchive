using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;

namespace WebAPI.Controllers
{
    public class UppyUploadController : BaseController<UploadController>
    {
        public UppyUploadController(IConfiguration config, ILogger<UploadController> log) : base(config, log)
        {
            targetFolder = $"{targetFolder}";
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadFile(IFormFileCollection fileupload)
        {
            if (fileupload.Count > 0)
            {
                var docfiles = new List<string>();
                foreach (IFormFile file in fileupload)
                {
                    FileInfo fi = new FileInfo(file.FileName);
                    string filepath = $"{tempFolder}/{fi.Name.Replace(fi.Extension, string.Empty)}{fi.Extension}";
                    using (FileStream fs = System.IO.File.Create(filepath))
                    {
                        byte[] bytes = new byte[file.Length];
                        await file.CopyToAsync(fs);
                    }
                }
                SetResponse(true, "File(s) uploaded successfully", null);
            }
            else
            {
                SetResponse(false, "Bad Request", null);
            }
            return Ok(responseData);
        }


    }
}
