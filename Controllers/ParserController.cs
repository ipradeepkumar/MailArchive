using Aspose.Words;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebAPI.Controllers;

namespace MailArchive.WebAPI.Controllers
{
    public class ParserController : BaseController<ParserController>
    {
        public ParserController(IConfiguration config, ILogger<ParserController> log) : base(config, log)
        {

        }

        [HttpPost("ParseDoc")]
        public async Task<IActionResult> Parse(string templateFilePath, string dataFilePath)
        {
            try
            {
                DataTable[] tables = ExcelHelper.Read(dataFilePath, true);

                StreamReader sr = new StreamReader(templateFilePath);
                var htmlString = sr.ReadToEnd();
                sr.Close();

                foreach (var row in tables[0].Rows)
                {
                    foreach (DataColumn col in tables[0].Columns)
                    {
                        htmlString.Replace(col.ColumnName, col.DefaultValue.ToString());
                    }
                    //save as word doc
                    Document wordDoc = new Document(htmlString.ToStream());
                    var outputPDF = wordDoc.Save($"{targetFolder}/ParsedPDF",SaveFormat.Pdf);
                    SetResponse(true, File(System.IO.File.ReadAllBytes($"{targetFolder}/ParsedPDF"), outputPDF.ContentType), null);
                }
            }
            catch (Exception ex)
            {
                SetResponse(false, null, ex.Message);
            }
            return Ok(responseData);

        }


    }
}
