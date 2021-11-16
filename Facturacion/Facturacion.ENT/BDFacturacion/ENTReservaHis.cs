using System;

namespace Facturacion.ENT
{
	public class ENTReservaHis
	{


		#region Propiedades Públicas
		/// <summary>
		/// Identificador del cabecero de la reservacion
		/// </summary>
		public long IdReservaCab { get; set; }

		/// <summary>
		/// Identificador del detalle de la reservacion.
		/// </summary>
		public int IdReservaDet { get; set; }

		/// <summary>
		/// Numero consecutivo de la foto registrada del detalle de la reservacion
		/// </summary>
		public int NumHistorico { get; set; }

		/// <summary>
		/// Fecha Hora en que se realizo el registro del historico
		/// </summary>
		public DateTime FechaHoraHistorico { get; set; }

		/// <summary>
		/// Orden en que se priorizan los cargos para asignar pagos
		/// </summary>
		public int Orden { get; set; }

		/// <summary>
		/// Contador del numero de cargo aplicado a la reservacion
		/// </summary>
		public int FeeNumber { get; set; }

		/// <summary>
		/// Identificador del pasajero en Navitaire
		/// </summary>
		public long PassengerID { get; set; }

		/// <summary>
		/// Identificador del segmento en Navitaire. En el caso de los FeeCharge donde no se tiene identificado el Segmento entonces se aplica la siguiente Regla. Se asigna el SegmentId en base al dato contenido en el ChargeDetail, en la mayoria tiene la ruta ORI-DES, con esto se puede identificar el segmento, los codigos EMI se cargan al primer segmento, los que no se puedan asignar de esta forma entonces, se valida la fecha de creacion del Cargo, si es antes de la fecha de salida (primer segmento) entonces se asigna al primer segmento, en caso de que el Cargo tenga fecha de creacion posterior al primer segmento y antes del segundo segmento se asignan al segundo segmento
		/// </summary>
		public long SegmentID { get; set; }

		/// <summary>
		/// Numero consecutivo del cargo
		/// </summary>
		public int ChargeNumber { get; set; }

		/// <summary>
		/// Identificador del tipo de cargo en Navitaire
		/// </summary>
		public int ChargeType { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public int IdFee { get; set; }

		/// <summary>
		/// Codigo del cargo (FeeCode)
		/// </summary>
		public string ChargeCode { get; set; }

		/// <summary>
		/// Descripcion del cargo
		/// </summary>
		public string ChargeDetail { get; set; }

		/// <summary>
		/// Codigo del agrupador del cargo (DisplayCode)
		/// </summary>
		public string TicketCode { get; set; }

		/// <summary>
		/// Codigo de la moneda
		/// </summary>
		public string CurrencyCode { get; set; }

		/// <summary>
		/// Monto del cargo
		/// </summary>
		public decimal ChargeAmount { get; set; }

		/// <summary>
		/// Codigo de moneda con que se hizo el cobro
		/// </summary>
		public string ForeignCurrencyCode { get; set; }

		/// <summary>
		/// Monto del cargo con la moneda local
		/// </summary>
		public decimal ForeignAmount { get; set; }

		/// <summary>
		/// Fecha en la que se aplica la compra, en caso de cambios de itinerario esta fecha se mantendra para los cargos que lo sustituyan
		/// </summary>
		public DateTime FechaAplicaCompra { get; set; }

		/// <summary>
		/// Porcentaje de IVA aplicado 
		/// </summary>
		public byte PorcIva { get; set; }

		/// <summary>
		/// Indica si se trata de un cargo de tarifa o un cargo por pasajero. Ej.TRAVELFEE o SSRFEE
		/// </summary>
		public string TipoCargo { get; set; }

		/// <summary>
		/// Codigo que indica en que tipo de acumulado se incluira el importe de este cargo
		/// </summary>
		public string TipoAcumulado { get; set; }

		/// <summary>
		/// Identificador del pago con que se cubrio el cargo
		/// </summary>
		public long IdPagosCab { get; set; }

		/// <summary>
		/// Bandera que indica cuando el cargo se cubrio con mas de un pago (Parcialidades)
		/// </summary>
		public bool EsPagoParcial { get; set; }

		/// <summary>
		/// Monto se se cubrio con el pago relacionado
		/// </summary>
		public decimal MontoPagado { get; set; }

		/// <summary>
		/// Bandera que indica si el cargo es facturable en funcion del Pago al cual esta vinculado
		/// </summary>
		public bool EsFacturable { get; set; }

		/// <summary>
		/// Indica si se encuentra facturado FT. Total, FP.Parcial. NF.No Facturado. FG. FacturaGlobal. NA. No aplica facturacion
		/// </summary>
		public string EstatusFacturacion { get; set; }

		/// <summary>
		/// Identificador de la factura
		/// </summary>
		public long IdFacturaCab { get; set; }

		/// <summary>
		/// FolioFactura (PaymentId) con el que se realizara la facturacion
		/// </summary>
		public long FolioPreFactura { get; set; }

		/// <summary>
		/// Id del concepto en que se factura el cargo
		/// </summary>
		public byte ClasFact { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public int IdConcepto { get; set; }

		/// <summary>
		/// Folio de la factura Global en que se incluye este cargo
		/// </summary>
		public long IdFolioFacturaGlobal { get; set; }

		/// <summary>
		/// Numero del inventario asignado al vuelo al que esta asignado este cargo
		/// </summary>
		public long IdVuelo { get; set; }

		/// <summary>
		/// Numero de Journey, que indica si el vuelo asignado es 1. Ida o 2. Regreso
		/// </summary>
		public byte NumJourney { get; set; }

		/// <summary>
		/// Estatus de abordaje del pasajero en el vuelo asignado
		/// </summary>
		public string LiftStatus { get; set; }

		/// <summary>
		/// Identificador del agente que registro el cargo
		/// </summary>
		public long CreatedAgentID { get; set; }

		/// <summary>
		/// Fecha de creacion del cargo
		/// </summary>
		public DateTime CreatedDate { get; set; }

		/// <summary>
		/// Fecha hora del registro de la Nota de Credito en formato de hora local
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
