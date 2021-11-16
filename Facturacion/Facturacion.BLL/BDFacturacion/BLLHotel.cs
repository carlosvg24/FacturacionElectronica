using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturacion.DAL;

namespace Facturacion.BLL
{
    public class BLLHotel : DALHotel
    {
        #region Constructores
        public BLLHotel(string idUsuario, string proceso)
        : base(BLLConfiguracion.Conexion)
        {
            base._idUsuario = idUsuario;
            base._proceso = proceso;
        }
        public BLLHotel()
        : base(BLLConfiguracion.Conexion)
        {
        }
        #endregion Constructores
        public string proceso { get; set; }
    }
}
