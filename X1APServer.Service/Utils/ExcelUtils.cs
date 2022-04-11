using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Utils
{
    public class ExcelUtils
    {
        public static void GenerateSheet(ref XSSFWorkbook workbook, string sheetName, List<string[]> data)
        {
            var sheet = workbook.CreateSheet(sheetName);

            for (int rowNum = 0; rowNum < data.Count; rowNum++)
            {
                IRow row = sheet.CreateRow(rowNum);

                for (int colNum = 0; colNum < data[rowNum].Length; colNum++)
                {
                    ICell cell = row.CreateCell(colNum);
                    cell.SetCellValue(data[rowNum][colNum]);
                }
            }
        }
    }
}
