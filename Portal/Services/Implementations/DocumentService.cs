using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Services.Implementations
{
    public enum DocumentType { CreateWaiterForm = 1, CreateBranchForm = 2 };

    public class DocumentService : IDocumentService
    {
        private const int DEFAULT_COLUMN_WIDTH = 30;
        private const short DEFAULT_HEADER_FONT_SIZE = 12;
        private const string DEFAULT_HEADER_FONT_NAME = "Calibri";

        public byte[] DownloadCreateUserForm(string[] headers)
        {

            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet();
            var header = sheet.CreateRow(0);

            var cellStyle = workbook.CreateCellStyle();
            cellStyle.DataFormat = (short)BuiltinFormats.GetBuiltinFormat("@");

            sheet.DefaultColumnWidth = DEFAULT_COLUMN_WIDTH;

            var headerFont = (XSSFFont)workbook.CreateFont();
            headerFont.FontHeightInPoints = DEFAULT_HEADER_FONT_SIZE;
            headerFont.FontName = DEFAULT_HEADER_FONT_NAME;
            headerFont.Boldweight = (short)FontBoldWeight.Bold;

            var headerStyle = workbook.CreateCellStyle();
            headerStyle.SetFont(headerFont);

            sheet.CreateFreezePane(0, 1, 0, 1); // sabit başlık

            for (int i = 0; i < headers.Length; i++)
            {
                sheet.SetColumnWidth(i, 30 * 300);
            }

            for (int i = 0; i < headers.Length; i++)
            {
                header.CreateCell(i).SetCellValue(headers[i]);
                header.GetCell(i).CellStyle = headerStyle;
            }

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);

            return ms.ToArray();
        }
    }
}
