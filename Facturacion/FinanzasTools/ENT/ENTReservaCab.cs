using System;

namespace FinanzasTools
{
	public class ENTReservaCab
	{


		#region Propiedades Públicas
		/// <summary>
		/// Identificador del cabecero de la reservacion
		/// </summary>
		public long IdReservaCab { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public byte IdEmpresa { get; set; }

		/// <summary>
		/// Identificador de la reservacion en Navitaire
		/// </summary>
		public long BookingID { get; set; }

		/// <summary>
		/// Codigo PNR
		/// </summary>
		public string RecordLocator { get; set; }

		/// <summary>
		/// Estatus de la reservacion
		/// </summary>
		public int Estatus { get; set; }

		/// <summary>
		/// Numero de Journeys que tiene la reservacion
		/// </summary>
		public byte NumJourneys { get; set; }

		/// <summary>
		/// Codigo del tipo de moneda en que se creo el PNR
		/// </summary>
		public string CurrencyCode { get; set; }

		/// <summary>
		/// Codigo de la empresa Navitaire que genero el PNR, siempre VB
		/// </summary>
		public string OwningCarrierCode { get; set; }

		/// <summary>
		/// Identificador del agente que creo la reservacion
		/// </summary>
		public long CreatedAgentID { get; set; }

		/// <summary>
		/// Fecha de creacion de la reservacion
		/// </summary>
		public DateTime CreatedDate { get; set; }

		/// <summary>
		/// Identificador del agente que modifico la reservacion
		/// </summary>
		public long ModifiedAgentID { get; set; }

		/// <summary>
		/// Fecha de la ultima modificacion del PNR
		/// </summary>
		public DateTime ModifiedDate { get; set; }

		/// <summary>
		/// Canal en que se genero la reservacion
		/// </summary>
		public byte ChannelTypeID { get; set; }

		/// <summary>
		/// Codigo de la organización donde se genero la reservacion
		/// </summary>
		public string CreatedOrganizationCode { get; set; }

		/// <summary>
		/// Monto total de la reservacion
		/// </summary>
		public decimal MontoTotal { get; set; }

		/// <summary>
		/// Monto Pagado de la reservacion
		/// </summary>
		public decimal MontoPagado { get; set; }

		/// <summary>
		/// Monto Facturado de la reservacion
		/// </summary>
		public decimal MontoFacturado { get; set; }

		/// <summary>
		/// Lista de IdFactura separados por '|'
		/// </summary>
		public string FoliosFacturacion { get; set; }

		/// <summary>
		/// Fecha hora del registro en formato de hora local
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }

		/// <summary>
		/// Fecha de la ultima actualizacion del registro en BD
		/// </summary>
		public DateTime FechaModificacion { get; set; }


		#endregion Propiedades Públicas

	}
}
