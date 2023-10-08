using Aspose.Words;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;

namespace MailArchive.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapDataController : BaseController<MapDataController>
    {
        public MapDataController(IConfiguration config, ILogger<MapDataController> log) : base(config, log) 
        {
            targetFolder = $"{targetFolder}/MappedTemplate";
        }
        [HttpPost("MapData")]
        public async Task<IActionResult> MapData([FromBody] string mappedContent)
        {
            try
            {
                byte[] data = Convert.FromBase64String(mappedContent);
                using (FileStream fs = new FileStream($"{targetFolder}/Template_{DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss")}.html", FileMode.Create))
                {
                    await fs.WriteAsync(data, 0, data.Length);
                    fs.Close();
                    fs.Flush();
                }
                SetResponse(true, "File saved successfully", null);
            }
            catch (Exception ex)
            {
                SetResponse(false, null, ex.Message);
            }
            return Ok(responseData);
        }
    }
}
