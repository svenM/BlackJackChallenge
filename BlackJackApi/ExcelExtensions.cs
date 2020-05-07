using BlackJackApi.Domain.DTO;
using OfficeOpenXml;
using System;

namespace BlackJackApi
{
    public static class ExcelExtensions
    {
        public static string CellValue(this ExcelWorksheet sheet, int row, int col)
        {
            return sheet.Cells[row, col].Value?.ToString();
        }

        public static DateTime CellDateValue(this ExcelWorksheet sheet, int row, int col, DateTime defaultDate)
        {
            if (string.IsNullOrWhiteSpace(sheet.CellValue(row, col))) return defaultDate;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("nl-BE");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("nl-BE");
            try
            {
                //try to parse as a date
                var value = sheet.Cells[row, col].Value;
                if (value is DateTime)
                {
                    return (DateTime)value;
                }
                else
                {
                    //parse the value as a number
                    double dateNum = double.Parse(value.ToString());
                    return DateTime.FromOADate(dateNum);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool CellEmpty(this ExcelWorksheet sheet, int row, int col)
        {
            return string.IsNullOrWhiteSpace(sheet.Cells[row, col].Value?.ToString());
        }


        public static CardRank? CellCardRank(this ExcelWorksheet sheet, int row, int col)
        {
            var value = sheet.CellValue(row, col);
            if (value == null) return null;

            return EnumExtensions.GetEnumFromValue<CardRank>(value.Trim());

        }
        public static PlayerAction? CellPlayerAction(this ExcelWorksheet sheet, int row, int col)
        {
            var value = sheet.CellValue(row, col);
            if (value == null) return null;

            return EnumExtensions.GetEnumFromValue<PlayerAction>(value.Trim());

        }
        public static int CellIntValue(this ExcelWorksheet sheet, int row, int col)
        {
            var value = sheet.Cells[row, col].Value;
            try
            {
                //parse the value as a number
                return int.Parse(value.ToString());
            }
            catch (Exception)
            {
                //TODO ILogger.WriteLine("Invalid value on " + sheet.Name + ", row " + row + ", value '" + value + "', expected number", ConsoleColor.Red);
                throw;
            }
        }

        public static bool IsEmpty(this ExcelWorksheet sheet, int row, int col)
        {
            return string.IsNullOrWhiteSpace(sheet.CellValue(row, col));
        }
    }
}
