using System;
using System.Collections; 
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;
using Facturacion.DAL.Portal;


namespace Facturacion.BLL.Portal
{
	public class BLLOpcionesmenuCat : DALOpcionesmenuCat
	{

		#region Constructores
		public BLLOpcionesmenuCat(string idUsuario,string proceso)
		: base(BLLConfiguracion.Conexion)
		{
			base._idUsuario = idUsuario;
			base._proceso = proceso;
		}
		public BLLOpcionesmenuCat()
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
