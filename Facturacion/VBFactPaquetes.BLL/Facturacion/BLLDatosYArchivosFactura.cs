using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VBFactPaquetes.Comun.Log;
using VBFactPaquetes.DAO.Facturacion;
using VBFactPaquetes.Model.Facturacion;

namespace VBFactPaquetes.BLL.Facturacion
{
    public class BLLDatosYArchivosFactura
    {
        private DataTable dtPagos = new DataTable();
        private DAOFacturacion daoFacturacion = new DAOFacturacion();
        string mensajeError = String.Empty;

        /*MÉTODO PARA MOVER LOS ARCHIVOS DE LA FACTURA.*/
        //Cambie el private por el public.
        public void MoverArchivosFactura(Pago pagosDTO, String TipoProceso, String nombreArchivo)
        {
            String rutaCarpetaDestino = String.Empty;
            String rutaArchivoDestino = String.Empty;

            try
            {
                /*EN CASO DE QUE EL TIPOPROCESO SEA "FALLO", MOVEMOS LOS ARCHIVOS QUE TUVIERON PROBLEMAS AL FACTURAR, EN LA CARPETA DE ARCHIVOSNOPROCESADOS*/
                if (TipoProceso == "Fallo")
                {
                    /*OBTENEMOS LA RUTA DESTINO DE LA CARPETA DONDE MOVEREMOS LOS ARCHIVOS QUE TIENEN PROBLEMAS PARA PROCESAR*/
                    rutaCarpetaDestino = pagosDTO.LstDatosGralDTO[0].CarpetaNoProcesados;

                    if (!Directory.Exists(rutaCarpetaDestino))
                        Directory.CreateDirectory(rutaCarpetaDestino);

                    rutaArchivoDestino = Path.Combine(rutaCarpetaDestino, nombreArchivo.Substring(nombreArchivo.LastIndexOf("\\") + 1));

                    System.IO.File.Move(nombreArchivo, rutaArchivoDestino);
                }
            }

            catch (Excepciones ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("pagosDTO", pagosDTO);
                throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                      MethodBase.GetCurrentMethod().Name,
                                      parametros,
                                      ex, Excepciones.TipoPortal.VivaPaquetes);

            }
        }


        /*MÉTODO PARA INSERTAR DATOS DE LA FACTURA.*/
        //Cambie el private por el public.
        public Boolean InsertarDatosFactura(Pago pagosDTO)
        {
            try
            {

                dtPagos = new DataTable();

                dtPagos = daoFacturacion.PagosFactura(pagosDTO, "A");

                if (dtPagos.Rows[0][0].ToString() != "ERROR")
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

            catch (Excepciones ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("pagosDTO", pagosDTO);
                throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                      MethodBase.GetCurrentMethod().Name,
                                       parametros,
                                      ex, Excepciones.TipoPortal.VivaPaquetes);


            }

            return true;
        }


        /*MÉTODO PARA ELIMINAR ARCHIVOS*/
        //Cambie el private por el public.
        public void EliminarArchivos(Pago pagosDTO)
        {
            if (Directory.Exists(pagosDTO.RutaArchivosCFDI))
            {
                Directory.Delete(pagosDTO.RutaArchivosCFDI, true);
                System.IO.File.Delete(pagosDTO.RutaArchivosCFDI + ".zip");
            }
        }
    }
}
