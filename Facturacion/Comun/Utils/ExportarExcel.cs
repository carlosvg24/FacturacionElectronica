using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Comun.Utils
{
    public class ExportarExcel
    {


        public void ExporttoExcelDataTable(DataTable table, String NombreReporte)
        {
            try
            {
                string Columnatableletter = IndexToColumn(table.Columns.Count);
                int columnTable = table.Columns.Count;
                int rowsTable = table.Rows.Count;
                using (XLWorkbook wb = new XLWorkbook())
                {



                    var ws = wb.Worksheets.Add(NombreReporte);
                    //El siguiente bloque agrega un encabezado
                    //ws.Cell("A1").Value = "VIVAAEROBUS " + NombreReporte + "_" + DateTime.Now.ToString("yyyy-MM-dd");
                    //ws.Range("A1:" + Columnatableletter + "1").Row(1).Merge();
                    //var rngTable3 = ws.Range("A1:" + Columnatableletter + "1");
                    //var rngHeaders3 = rngTable3.Range("A1:" + Columnatableletter + "1"); // The address is relative to rngTable (NOT the worksheet)
                    //rngHeaders3.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    //rngHeaders3.Style.Font.Bold = true;
                    //rngHeaders3.Style.Font.FontColor = XLColor.White;
                    //rngHeaders3.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    //rngHeaders3.Style.Fill.BackgroundColor = XLColor.JungleGreen;


                    var tableWithData = ws.Cell(1, 1).InsertTable(table.AsEnumerable());


                    var rngTable2 = ws.Range("A1:" + Columnatableletter + rowsTable.ToString());
                    var rngHeaders2 = rngTable2.Range("A1:" + Columnatableletter + "1"); // The address is relative to rngTable (NOT the worksheet)
                    rngHeaders2.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    rngHeaders2.Style.Font.Bold = true;
                    rngHeaders2.Style.Font.FontColor = XLColor.White;
                    rngHeaders2.Style.Fill.BackgroundColor = XLColor.GreenRyb;

                    ws.AutoFilter.Enabled = false;
                    ws.Columns().AdjustToContents();
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + NombreReporte + "_" + DateTime.Now.ToString("ddMMyyyy_HHmm") + ".xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        bool aceptaEnvio = false;
                        try
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                            aceptaEnvio = true;
                        }
                        catch (Exception)
                        {
                        }
                        if (aceptaEnvio)
                        {
                            HttpContext.Current.Response.Flush();
                            HttpContext.Current.Response.End();
                        }
                        else
                        {
                            throw new Exception("No fue posible generar el archivo, intente agregando mas filtros...");

                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        const int ColumnBase = 26;
        const int DigitMax = 7; // ceil(log26(Int32.Max))
        const string Digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public string IndexToColumn(int index)
        {
            if (index <= 0)
                throw new IndexOutOfRangeException("index must be a positive number");

            if (index <= ColumnBase)
                return Digits[index - 1].ToString();

            var sb = new StringBuilder().Append(' ', DigitMax);
            var current = index;
            var offset = DigitMax;
            while (current > 0)
            {
                sb[--offset] = Digits[--current % ColumnBase];
                current /= ColumnBase;
            }
            return sb.ToString(offset, DigitMax - offset);
        }
    }
}
