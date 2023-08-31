using MoneyManager.Client.ViewModels;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MoneyManager.Client.Services.XlsxFileService
{
    public class XmlReportGenerateService
    {
        public byte[] CreateXmlDoc(List<RecordVM> data)
        {
            byte[] byteArray;

            using MemoryStream mem = new();
            using SpreadsheetDocument document = SpreadsheetDocument.Create(mem, SpreadsheetDocumentType.Workbook);

            WorkbookPart workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet();

            Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

            Sheet sheet = new()
            {
                Id = workbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Finance"
            };
            sheets.Append(sheet);

            WriteToExcel(worksheetPart, data);

            workbookPart.Workbook.Save();
            document.Close();
            byteArray = mem.ToArray();

            return byteArray;


        }

        private static void WriteToExcel(WorksheetPart worksheetPart, List<RecordVM> data)
        {
            SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

            Row row = new();
            row.Append(
                new Cell() { CellValue = new CellValue("Name"), DataType = CellValues.String },
                new Cell() { CellValue = new CellValue("Category"), DataType = CellValues.String },
                new Cell() { CellValue = new CellValue("Amount"), DataType = CellValues.String },
                new Cell() { CellValue = new CellValue("Date"), DataType = CellValues.String });
            sheetData.Append(row);

            foreach (var item in data)
            {
                row = new();
                row.Append(
                    new Cell() { CellValue = new CellValue(item.Name), DataType = CellValues.String },
                    new Cell() { CellValue = new CellValue(item.Category?.Name ?? ""), DataType = CellValues.String },
                    new Cell() { CellValue = new CellValue(item.Amount), DataType = CellValues.Number },
                    new Cell() { CellValue = new CellValue(item.TransactionDate.ToString("dd/MM/yyyy")), DataType = CellValues.String });
                sheetData.Append(row);
            }

            worksheetPart.Worksheet.Save();
        }
    }
}
