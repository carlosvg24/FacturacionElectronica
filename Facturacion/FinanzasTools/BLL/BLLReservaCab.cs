using System;
using System.Collections; 
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;




namespace FinanzasTools
{
	public class BLLReservaCab : DALReservaCab
	{

		#region Constructores
		public BLLReservaCab(string idUsuario,string proceso)
		: base(BLLConfiguracion.Conexion)
		{
			base._idUsuario = idUsuario;
			base._proceso = proceso;
		}
		public BLLReservaCab()
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
