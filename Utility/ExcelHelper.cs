using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MailArchive.WebAPI
{
    public class ExcelHelper
    {
        public static DataTable[] Read(string path, bool isHeaderOnTopRow = false)
        {
            try
            {
                using (var doc = SpreadsheetDocument.Open(path, false))
                {
                    var sheets = doc.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                    DataTable[] dtArray = new DataTable[sheets.ToList().Count];
                    int counti = 0;
                    foreach (Sheet sheet in sheets)
                    {
                        var dt = new DataTable();

                        var relationshipId = sheet.Id.Value;
                        var worksheetPart = (WorksheetPart)doc.WorkbookPart.GetPartById(relationshipId);
                        var workSheet = worksheetPart.Worksheet;
                        var sheetData = workSheet.GetFirstChild<SheetData>();
                        var rows = sheetData.Descendants<Row>().ToList();

                        int rowIndex = 0;
                        foreach (var row in rows) //this will also include your header row...
                        {
                            var tempRow = dt.NewRow();

                            var colCount = row.Descendants<Cell>().Count();
                            int colIndex = 0;
                            foreach (var cell in row.Descendants<Cell>())
                            {
                                var index = GetIndex(cell.CellReference);
                                index = (index < 0 ? colIndex++ : index);

                                // Add Columns
                                for (var i = dt.Columns.Count; i <= index; i++)
                                    dt.Columns.Add();

                                if (isHeaderOnTopRow && rowIndex == 0)
                                {
                                    string heading = GetCellValue(doc, cell);
                                    heading = (heading.Length > 0 ? heading : $"Column{index + 1}");
                                    dt.Columns[index].ColumnName = heading;
                                }
                                else
                                {
                                    tempRow[index] = GetCellValue(doc, cell);
                                }
                            }
                            if (rowIndex > 0 || isHeaderOnTopRow == false)
                            {
                                dt.Rows.Add(tempRow);
                            }
                            rowIndex++;
                        }
                        dtArray[counti++] = dt;
                    }
                    return dtArray;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }

        private static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            var stringTablePart = document.WorkbookPart.SharedStringTablePart;
            var value = cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                return stringTablePart.SharedStringTable.ChildElements[int.Parse(value)].InnerText;

            return value;
        }

        public static int GetIndex(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return -1;

            int index = 0;
            foreach (var ch in name)
            {
                if (char.IsLetter(ch))
                {
                    int value = ch - 'A' + 1;
                    index = value + index * 26;
                }
                else
                    break;
            }

            return index - 1;
        }

        public static void ExportDataSet(DataSet ds, string destination)
        {
            try
            {
                using (var workbook = SpreadsheetDocument.Create(destination, SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();

                    workbook.WorkbookPart.Workbook = new Workbook();

                    workbook.WorkbookPart.Workbook.Sheets = new Sheets();

                    foreach (System.Data.DataTable table in ds.Tables)
                    {

                        var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                        var sheetData = new SheetData();
                        sheetPart.Worksheet = new Worksheet(sheetData);

                        Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<Sheets>();
                        string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                        uint sheetId = 1;
                        if (sheets.Elements<Sheet>().Count() > 0)
                        {
                            sheetId =
                                sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                        }

                        Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                        sheets.Append(sheet);

                        Row headerRow = new Row();

                        List<String> columns = new List<string>();
                        foreach (System.Data.DataColumn column in table.Columns)
                        {
                            columns.Add(column.ColumnName);

                            Cell cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue(column.ColumnName);
                            headerRow.AppendChild(cell);
                        }


                        sheetData.AppendChild(headerRow);

                        foreach (System.Data.DataRow dsrow in table.Rows)
                        {
                            Row newRow = new Row();
                            foreach (String col in columns)
                            {
                                Cell cell = new Cell();
                                cell.DataType = CellValues.String;
                                cell.CellValue = new CellValue(dsrow[col].ToString()); //
                                newRow.AppendChild(cell);
                            }

                            sheetData.AppendChild(newRow);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void saveDataTablesToExcel(DataTable[] dataTables, string saveToFilePath)
        {
            // Create a DataSet
            DataSet dataSet = new DataSet("Tables");
            // We can add multiple DataTable to DataSet
            foreach (DataTable dt in dataTables)
            {
                dataSet.Tables.Add(dt);
            }

            ExportDataSet(dataSet, saveToFilePath);
        }

    }
}
