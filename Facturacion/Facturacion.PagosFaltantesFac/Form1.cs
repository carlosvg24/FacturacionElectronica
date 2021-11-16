using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Facturacion.BLL;
using Facturacion.BLL.ProcesoFacturacion;
using System.Data.SqlClient;
using Comun.Security;
using System.Configuration;
using Facturacion.BLL;
//using Facturacion.DAL;
//using Facturacion.ENT;

namespace Facturacion.PagosFaltantesFac
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CargarComboProcesos();
        }


        public int Proceso { get; set; }

        private void CargarComboProcesos()
        {
            cmbProceso.Items.Insert(0, "Seleccione...");
            cmbProceso.Items.Insert(1, "Pagos Faltantes Navitaire");
            cmbProceso.Items.Insert(2, "Generar Biblioteca CFDI");
            cmbProceso.SelectedIndex = 0;
        }

        private void btnProcesar_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                LimpiarLog();

                switch (Proceso)
                {

                    case 1:
                        //ProcesarPagosFaltantes();
                        break;
                    case 2:
                        GenerarCFDI(dtpFechaIni.Value, dtpFechaFin.Value);
                        break;
                    default:
                        MessageBox.Show("Debe seleccionar algún proceso...", "Herramientas Facturación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Herramientas Facturación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }




        }

        private void RegLog(string mensaje)
        {
            lstResumen.Items.Add(string.Format("{0} {1}", DateTime.Now, mensaje));
        }

        private void LimpiarLog()
        {
            lstResumen.Items.Clear();
        }

        private void ProcesarPagosFaltantes()
        {
            RegLog("Iniciando proceso...");
            BLLProcesarPagos bllProcesarPago = new BLL.ProcesoFacturacion.BLLProcesarPagos();
            //Sincroniza los pagos con la BD de Navitaire
            bllProcesarPago.SincronizarPagosNavitaire();

            //Procesa los pagos faltantes
            bllProcesarPago.ProcesarPagosFaltantes();
            RegLog("Finalizando Proceso...");
        }

        private void GenerarCFDI(DateTime fechaIni, DateTime fechaFin)
        {
            RegLog("Iniciando proceso...");

            try
            {
                List<string> Errores = new List<string>();
                while (fechaIni <= fechaFin)
                {
                    RegLog(string.Format("Recuperando informacion del dia {0}", fechaIni.ToShortDateString()));
                    this.Refresh();
                    DataTable dtCFDI = new DataTable();
                    dtCFDI = RecuperarCFDIPorFecha(fechaIni, fechaIni);
                    BLLPDFCFDI bllcfdi = new BLLPDFCFDI();

                    int totalReg = 0;
                    totalReg = dtCFDI.Rows.Count;
                    RegLog(string.Format("Registros por Procesar {0}", totalReg.ToString()));
                    int cont = 0;
                    int avance = 0;
                    foreach (DataRow dr in dtCFDI.Rows)
                    {
                        long idPeticionPAC = (long)dr["IdPeticionPAC"];
                        DateTime fechaTimbrado = (DateTime)dr["FechaTimbrado"];
                        string pnr = dr["PNR"].ToString();
                        string tipoComprobante = dr["TipoComprobante"].ToString();
                        string ruta = dr["Ruta"].ToString();
                        string nombreArchivo = dr["NombreArchivo"].ToString();
                        string xmlCfdi = dr["CFD_ComprobanteStr"].ToString();
                        string cFD_CadenaOriginal = dr["CFD_CadenaOriginal"].ToString();


                        try
                        {
                            bllcfdi.GeneraArchivoCFDI(xmlCfdi, ruta, nombreArchivo + ".xml");

                            BLLXmlFtpReg bllFtp = new BLLXmlFtpReg();
                            bllFtp.IdPeticionPAC = idPeticionPAC;
                            bllFtp.FechaTimbrado = fechaTimbrado;
                            bllFtp.RutaCFDI = ruta + nombreArchivo + ".xml";

                            bllcfdi.GeneraArchivoCFDI(xmlCfdi, ruta, nombreArchivo + ".xml");
                            if (tipoComprobante == "I")
                            {
                                bllcfdi.GeneraPDFFactura33(xmlCfdi, cFD_CadenaOriginal, pnr, ruta, nombreArchivo);
                                bllFtp.RutaPDF = ruta + nombreArchivo + ".pdf";
                            }

                            bllFtp.Agregar();

                        }
                        catch (Exception ex)
                        {
                            Errores.Add(idPeticionPAC.ToString() + "|" + ex.Message);
                        }
                        finally
                        {
                            cont++;
                            avance = ((cont * 100) / totalReg);
                            pbAvance.Value = avance;
                            //
                        }
                    }
                    RegLog(string.Format("Finaliza proceso de CFDI del dia {0}", fechaIni.ToShortDateString()));
                    this.Refresh();
                    fechaIni = fechaIni.AddDays(1);
                }



            }
            catch (Exception ex)
            {

                throw new Exception("Error: " + ex.Message);
            }

            RegLog("Finalizando Proceso...");
        }

        private void cmbProceso_SelectedIndexChanged(object sender, EventArgs e)
        {
            Proceso = cmbProceso.SelectedIndex;

            switch (Proceso)
            {
                case 1:
                    gbCFDI.Visible = false;
                    break;
                case 2:
                    gbCFDI.Visible = true;
                    break;
                default:
                    break;
            }

        }

        public DataTable RecuperarCFDIPorFecha(DateTime fechaIni, DateTime fechaFin)
        {

            Encrypt encrypt = new Encrypt();
            SqlConnection _conexion = new SqlConnection();
            String conString = "";
            conString = encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBFacturacion"].ToString());


            SqlConnection conn = new SqlConnection(conString);
            DataTable dtResultado = new DataTable();
            SqlCommand cmm = new SqlCommand();
            cmm.CommandType = CommandType.StoredProcedure;

            cmm.CommandText = "[dbo].[uspFac_GetXMLCFDITotalPorFecha]";
            //cmm.CommandText = "[dbo].[uspFac_GetXMLCFDIPorFecha]";
            cmm.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
            SqlParameter param1 = new SqlParameter("@FECHAINI", SqlDbType.VarChar);
            param1.Value = fechaIni.Year > 1900 ? fechaIni.ToString("yyyy-MM-dd") : "";
            cmm.Parameters.Add(param1);

            SqlParameter param2 = new SqlParameter("@FECHAFIN", SqlDbType.VarChar);
            param2.Value = fechaFin.Year > 1900 ? fechaFin.ToString("yyyy-MM-dd") : "";
            cmm.Parameters.Add(param2);

            cmm.Connection = conn;
            bool cerrarConexion = false;

            try
            {
                if (conn.State.Equals(ConnectionState.Closed))
                {
                    conn.Open();
                    cerrarConexion = true;
                }
                SqlDataAdapter da = new SqlDataAdapter(cmm);
                DataSet dsResult = new DataSet();
                da.Fill(dsResult, "dtResultado");
                dtResultado = dsResult.Tables["dtResultado"];

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmm.Dispose();
                if (cerrarConexion)
                {
                    conn.Close();
                }
            }
            return dtResultado;
        }


    }
}
