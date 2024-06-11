using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aspose.Words;

namespace WebAPI.Controllers
{
    public class ConvertorController : BaseController<ConvertorController>
    {
        public ConvertorController(IConfiguration config, ILogger<ConvertorController> log) : base(config, log)
        {
            targetFolder = $"{targetFolder}/Template";
        }
        [HttpPost("ConvertDoc")]
        public async Task<IActionResult> Convert(string filename)
        {
            try
            {
                FileInfo fi = new FileInfo(filename);
                string tempWordDocPath = $"{tempFolder}/{fi.Name}";
                string tempHtmlPath = $"{targetFolder}/{fi.Name.Replace(fi.Extension, string.Empty)}.html";
                //save word file in temp path
                using (FileStream fs = System.IO.File.Create(tempWordDocPath))
                {
                    byte[] bytes = new byte[fi.Length];
                    int bytesRead = 0;
                    while ((bytesRead = await Request.Body.ReadAsync(bytes, 0, bytes.Length)) > 0)
                    {
                        fs.Write(bytes, 0, bytesRead);
                    }
                }
                //convert word to html
                Document doc = new Document(tempWordDocPath);
                //Save to HTML
                var outHtml = doc.Save(tempHtmlPath, SaveFormat.Html);
                //send back html content to UI
                var html = System.IO.File.ReadAllText(tempHtmlPath);
                responseData = SetResponse(true, base.Content(html, outHtml.ContentType), null);
            }
            catch (Exception ex)
            {
                responseData = SetResponse(false, null, ex.Message);
            }
            return Ok(responseData);
        }
    }
}
