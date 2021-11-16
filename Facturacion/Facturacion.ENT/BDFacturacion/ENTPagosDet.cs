using System;

namespace Facturacion.ENT
{
	public class ENTPagosDet
	{


		#region Propiedades Públicas
		/// <summary>
		/// Identificador del pago registrado con que se vincula
		/// </summary>
		public long IdPagosCab { get; set; }

		/// <summary>
		/// Identificador del Pago en Navitaire
		/// </summary>
		public long PaymentID { get; set; }

		/// <summary>
		/// Identificador de la reservacion en Navitaire
		/// </summary>
		public long BookingID { get; set; }

		/// <summary>
		/// Fecha en que se realizo el pago
		/// </summary>
		public DateTime FechaPago { get; set; }

		/// <summary>
		/// Codigo del Metodo o forma de pago
		/// </summary>
		public string PaymentMethodCode { get; set; }

		/// <summary>
		/// Codigo de moneda del pago
		/// </summary>
		public string CurrencyCode { get; set; }

		/// <summary>
		/// Monto del pago
		/// </summary>
		public decimal PaymentAmount { get; set; }

		/// <summary>
		/// Codigo de moneda en que se cobro el pago
		/// </summary>
		public string CollectedCurrencyCode { get; set; }

		/// <summary>
		/// Monto cobrado en el codigo recolectado
		/// </summary>
		public decimal CollectedAmount { get; set; }

		/// <summary>
		/// Identificador del agente que realizo el cobro
		/// </summary>
		public long IdAgente { get; set; }

		/// <summary>
		/// Fecha en que se registro en BD Facturacion
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
