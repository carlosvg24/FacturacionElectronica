﻿using System;
using System.Collections; 
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;


using Facturacion.ENT;
using Facturacion.DAL;


namespace Facturacion.BLL
{
	public class BLLGlobalpagosnoenviadosReg : DALGlobalpagosnoenviadosReg
	{

		#region Constructores
		public BLLGlobalpagosnoenviadosReg(string idUsuario,string proceso)
		: base(BLLConfiguracion.Conexion)
		{
			base._idUsuario = idUsuario;
			base._proceso = proceso;
		}
		public BLLGlobalpagosnoenviadosReg()
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
