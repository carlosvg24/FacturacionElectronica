using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBFactPaquetes.DAO;

namespace VBFactPaquetes.BLL
{
    public class BLLXml_WS_Facturalo : DAOXml_WS_Facturalo
    {

        #region Constructores
        public BLLXml_WS_Facturalo(string idUsuario, string proceso)
        : base(BLLConfiguracion.Conexion)
        {
            base._idUsuario = idUsuario;
            base._proceso = proceso;
        }
        public BLLXml_WS_Facturalo()
        : base(BLLConfiguracion.Conexion)
        {
        }
        #endregion Constructores

        #region Propiedades Privadas
        #endregion Propiedades Privadas

        #region Métodos Privados
        #endregion Métodos Privados

        #region Propiedades Públicas
        #endregion Propiedades Públicas

        #region Métodos Públicos
        #endregion Métodos Públicos

    }
}
