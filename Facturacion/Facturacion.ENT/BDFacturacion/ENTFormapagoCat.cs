using System;

namespace Facturacion.ENT
{
	public class ENTFormapagoCat
	{


		#region Propiedades Públicas
		/// <summary>
		/// Identificador de la forma de pago
		/// </summary>
		public int IdFormaPago { get; set; }

		/// <summary>
		/// Codigo del metodo de pago utilizado en Navitaire
		/// </summary>
		public string PaymentMethodCode { get; set; }

		/// <summary>
		/// Descripcion de la forma de pago
		/// </summary>
		public string Descripcion { get; set; }

		/// <summary>
		/// Codigo asignado por el SAT para este metodo de pago
		/// </summary>
		public string CveFormaPagoSAT { get; set; }

		/// <summary>
		/// Bandera que indica si el pago es facturable o no
		/// </summary>
		public bool EsFacturable { get; set; }

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

		/// <summary>
		/// 
		/// </summary>
		public bool EsFacturablePortalAdmin { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool EsFacturableGlobal { get; set; }


		#endregion Propiedades Públicas

	}
}
