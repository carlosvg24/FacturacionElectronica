using System;

namespace FinanzasTools
{
	public class ENTPagosCab
	{


		#region Propiedades Públicas
		/// <summary>
		/// Identificador del pago registrado
		/// </summary>
		public long IdPagosCab { get; set; }

		/// <summary>
		/// Identificador de la reservacion en Navitaire
		/// </summary>
		public long BookingID { get; set; }

		/// <summary>
		/// Identificador del Pago en Navitaire
		/// </summary>
		public long PaymentID { get; set; }

		/// <summary>
		/// Fecha en que se realizo el pago
		/// </summary>
		public DateTime FechaPago { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaPagoUTC { get; set; }

		/// <summary>
		/// Identificador de la forma de pago
		/// </summary>
		public int IdFormaPago { get; set; }

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
		/// Monto del pago que se aplica a los cargos, en caso de que el pago tenga algun ajuste
		/// </summary>
		public decimal MontoPorAplicar { get; set; }

		/// <summary>
		/// Codigo de moneda en que se cobro el pago
		/// </summary>
		public string CollectedCurrencyCode { get; set; }

		/// <summary>
		/// Monto cobrado en el codigo recolectado
		/// </summary>
		public decimal CollectedAmount { get; set; }

		/// <summary>
		/// Monto del tipo de cambio
		/// </summary>
		public decimal TipoCambio { get; set; }

		/// <summary>
		/// Estatus del Pago
		/// </summary>
		public int Estatus { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long ParentPaymentID { get; set; }

		/// <summary>
		/// Indica si este pago fue dividido en otro PNR
		/// </summary>
		public bool EsPagoDividido { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool EsPagoPadre { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool EsPagoHijo { get; set; }

		/// <summary>
		/// Bandera que indica si el pago es facturable
		/// </summary>
		public bool EsFacturable { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool EsParaAplicar { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool EsFacturado { get; set; }

		/// <summary>
		/// Id de la factura con que se vinculo el pago
		/// </summary>
		public long IdFacturaCab { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long IdFacturaGlobal { get; set; }

		/// <summary>
		/// Folio de la prefactura, en caso de multipagos tendra el PaymentId del pago mas representativo
		/// </summary>
		public long FolioPrefactura { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long FolioFactura { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaFactura { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaFacturaUTC { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string VersionFacturacion { get; set; }

		/// <summary>
		/// Monto pagado por concepto de Tarifa
		/// </summary>
		public decimal MontoTarifa { get; set; }

		/// <summary>
		/// Monto Total pagado por concepto de Servicios Adicionales
		/// </summary>
		public decimal MontoServAdic { get; set; }

		/// <summary>
		/// Monto Total pagado por concepto de TUA
		/// </summary>
		public decimal MontoTUA { get; set; }

		/// <summary>
		/// Monto total pagado por concepto de Otros Cargos
		/// </summary>
		public decimal MontoOtrosCargos { get; set; }

		/// <summary>
		/// Monto total pagado por concepto de IVA
		/// </summary>
		public decimal MontoIVA { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public decimal MontoTotal { get; set; }

		/// <summary>
		/// Identificador del agente que realizo el cobro
		/// </summary>
		public long IdAgente { get; set; }

		/// <summary>
		/// Fecha en que se registro en BD Facturacion
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }

		/// <summary>
		/// Fecha en que se realizo el ultimo movimiento a la reservacion
		/// </summary>
		public DateTime FechaUltimaActualizacion { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long IdNotaCreditoCab { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string LocationCode { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string LugarExpedicion { get; set; }


		#endregion Propiedades Públicas

	}
}
