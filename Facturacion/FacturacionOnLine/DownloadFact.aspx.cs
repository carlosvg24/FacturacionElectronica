using Facturacion.BLL;
using Facturacion.BLL.ProcesoFacturacion;
using Facturacion.ENT;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FacturacionOnLine
{
    public partial class DownloadFact : System.Web.UI.Page
    {
        public string RutaArchivoDescarga
        {
            get
            {
                if (Session["RutaArchivoDescarga"] == null)
                {
                    return "";
                }
                else
                {
                    return Session["RutaArchivoDescarga"].ToString();
                }
            }

            set
            {
                Session["RutaArchivoDescarga"] = value;
            }
        }




        protected void Page_Load(object sender, EventArgs e)
        {

            //Se genera el archivoZip Bajo demanda
            BLLFacturacion bllFactura = new BLLFacturacion();
            GeneraZipDescarga(RutaArchivoDescarga);
        }




        private void GeneraZipDescarga(string rutaArchivos)
        {
            DirectoryInfo carpeta = new DirectoryInfo(rutaArchivos);
            if (carpeta.Exists)
            {
                string zipName = String.Format("CFDI_{0}.zip", carpeta.Name);
                Response.Clear();
                //Response.ClearContent();
                //Response.ClearHeaders();
                Response.ContentType = "application/zip";
                Response.AppendHeader("content-disposition", "attachment; filename=" + zipName);

                using (ZipFile zip = new ZipFile())
                {

                    foreach (FileInfo archivo in carpeta.GetFiles())
                    {
                        if (archivo.Extension.ToString().ToLower().Trim() != ".zip")
                            zip.AddEntry(archivo.Name, archivo.OpenRead());
                    }

                    foreach (DirectoryInfo subCarpeta in carpeta.GetDirectories())
                    {
                        foreach (FileInfo archivo in subCarpeta.GetFiles())
                        {
                            if (archivo.Extension.ToString().ToLower().Trim() != ".zip")
                                zip.AddEntry(archivo.Name, archivo.OpenRead());
                        }
                    }

                    zip.Save(carpeta + "\\" + zipName);//.Save(Response.OutputStream); 
                }

                Response.TransmitFile(carpeta + "\\" + zipName);
                Response.Flush();
                Response.End();

            }

        }

    }
}