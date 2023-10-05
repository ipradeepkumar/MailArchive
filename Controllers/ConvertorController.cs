using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spire.Doc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConvertorController : BaseController
    {
        private IConfiguration configuration;
        private readonly ILogger<ConvertorController> _logger;
        private string _tempFolder;
        private readonly ResponseContext _responseData;
        public ConvertorController(IConfiguration _configuration, ILogger<ConvertorController> logger)
        {
            this.configuration = _configuration;
            _logger = logger;
            _tempFolder = configuration["TempFolder"];
            _responseData = new ResponseContext();
        }
        [HttpPost("ConvertDoc")]
        public async Task<IActionResult> Convert(string filename)
        {
            try
            {
                FileInfo fi = new FileInfo(filename);
                string tempWordDocPath = $"{_tempFolder}/{fi.Name}";
                string tempHtmlPath = $"{fi.Name.Replace(fi.Extension, string.Empty)}.html";
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
                using (Document doc = new Document())
                {
                    //Load a Word document
                    doc.LoadFromFile(tempWordDocPath);

                    //Embed images in generated HTML file
                    doc.HtmlExportOptions.ImageEmbedded = true;

                    //Save to HTML
                    doc.SaveToFile(tempHtmlPath, FileFormat.Html);
                }
                //send back html content to UI
                var html = System.IO.File.ReadAllText(tempHtmlPath);
                return base.Content(html, "text/html");
            }
            catch (Exception ex)
            {
                _responseData.ErrorMessage = ex.Message;
                _responseData.IsSuccess = false;
            }
            return Ok(_responseData);
        }
    }
}
