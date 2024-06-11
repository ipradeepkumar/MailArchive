using Aspose.Words;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;
using Aspose.Words;
using System.Data;

namespace MailArchive.WebAPI.Controllers
{
  
    public class MapDataController : BaseController<MapDataController>
    {
        public MapDataController(IConfiguration config, ILogger<MapDataController> log) : base(config, log) 
        {
        }
        [HttpGet("GetData")]
        public MapDataModel GetData(string? dataFile = null, string? templateFile = null)
        {

            var response = new MapDataModel();

            if (templateFile == null || dataFile == null) return response;

            response.Html = GetTemplateAsHtml(templateFile);
            response.Columns = GetDataColumns(dataFile);
            return response;
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

        private string GetTemplateAsHtml(string templateFileName)
        {
            var htmlContent = string.Empty;
            try
            {
                FileInfo fi = new FileInfo($"{targetFolder}/Template/{templateFileName}");
                //string tempWordDocPath = $"{tempFolder}/{fi.Name}";
                string tempHtmlPath = $"{targetFolder}/{fi.Name.Replace(fi.Extension, string.Empty)}.html";
                Document doc = new Document(fi.FullName);
                //Save to HTML
                var outHtml = doc.Save(tempHtmlPath, SaveFormat.Html);
                //send back html content to UI
                htmlContent =  System.IO.File.ReadAllText(tempHtmlPath);
            }
            catch (Exception ex)
            {
                responseData = base.SetResponse(false, null, ex.Message);
            }
            return htmlContent;
            
        }
        
        private string[] GetDataColumns(string dataFileName)
        {
            DataTable[] tables = ExcelHelper.Read($"{targetFolder}/Data/{dataFileName}", true);
            string[] columnNames = (from dc in tables[0].Columns.Cast<DataColumn>()
                                    select dc.ColumnName).ToArray();
            return columnNames;
        }
    }
}
