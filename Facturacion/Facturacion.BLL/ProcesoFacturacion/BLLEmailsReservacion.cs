using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.BLL.ProcesoFacturacion
{
    public class BLLEmailsReservacion
    {
        public DataTable ObtenerPagosSinEmail()
        {
            DataTable dt = new DataTable();
            try
            {
                BLLFacturacion fact = new BLLFacturacion();
                dt = fact.GetPagosSinFacturarSinEmail();

            } catch(Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public void ActualizarEmailReservacion()
        {

        }
    }
}
