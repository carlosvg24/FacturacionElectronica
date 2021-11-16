using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanzasTools
{
    public class DALFinanzas:ENTFinanzas
    {
        #region Propiedades Privadas
        protected SqlConnection _conexion;
        #endregion Propiedades Privadas


        #region Constructores
        public DALFinanzas(SqlConnection conexion)
        {
            _conexion = conexion;
        }
        #endregion Constructores

        #region Miembros de IAccesoDatos
       


       
        #endregion Miembros de IAccesoDatos
    }
}
