using System;

namespace Facturacion.ENT
{
	public class ENTVuelosCab
	{


		#region Propiedades Públicas
		/// <summary>
		/// 
		/// </summary>
		public long IdVuelo { get; set; }

		/// <summary>
		/// Numero del inventario asignado al vuelo al que esta asignado este cargo
		/// </summary>
		public long InventoryLegId { get; set; }

		/// <summary>
		/// Codigo del vuelo donde indica la fecha, city par y numero de vuelo
		/// </summary>
		public string InventoryLegKey { get; set; }

		/// <summary>
		/// Fecha en que sale el vuelo
		/// </summary>
		public DateTime DepartureDate { get; set; }

		/// <summary>
		/// Codigo de la empresa VB
		/// </summary>
		public string CarrierCode { get; set; }

		/// <summary>
		/// Numero de vuelo
		/// </summary>
		public string FlightNumber { get; set; }

		/// <summary>
		/// Codigo del aeropuerto de donde sale
		/// </summary>
		public string DepartureStation { get; set; }

		/// <summary>
		/// Fecha hora de salida
		/// </summary>
		public DateTime STD { get; set; }

		/// <summary>
		/// Terminal de donde sale el vuelo
		/// </summary>
		public string DepartureTerminal { get; set; }

		/// <summary>
		/// Codigo del aeropuerto donde llega el vuelo
		/// </summary>
		public string ArrivalStation { get; set; }

		/// <summary>
		/// Fecha Hora de llegada del vuelo
		/// </summary>
		public DateTime STA { get; set; }

		/// <summary>
		/// Terminal donde llega el vuelo
		/// </summary>
		public string ArrivalTerminal { get; set; }

		/// <summary>
		/// Indica el origen y destino del vuelo 
		/// </summary>
		public string CityPair { get; set; }


		#endregion Propiedades Públicas

	}
}
