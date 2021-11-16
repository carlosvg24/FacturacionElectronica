using System;

namespace Facturacion.ENT
{
	public class ENTFeeCat
	{


		#region Propiedades Públicas
		/// <summary>
		/// 
		/// </summary>
		public int IdFee { get; set; }

		/// <summary>
		/// Clave del FeeCode
		/// </summary>
		public string FeeCode { get; set; }

		/// <summary>
		/// Codigo del grupo del Fee
		/// </summary>
		public string DisplayCode { get; set; }

		/// <summary>
		/// Descripcion del Fee
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Identificador del concepto en que clasifica el tipo de Fee para mostrarlo en la facturacion ej. Tarifa, Servicio Adic. Cargo, etc
		/// </summary>
		public byte ClasFact { get; set; }

		/// <summary>
		/// Bandera de eliminacion logica, 0. Inactivo. 1.Activo
		/// </summary>
		public bool Activo { get; set; }

		/// <summary>
		/// Usuario que realizó el registro
		/// </summary>
		public int UsuarioRegistro { get; set; }

		/// <summary>
		/// Fecha Hora en que se realizo el registro
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
