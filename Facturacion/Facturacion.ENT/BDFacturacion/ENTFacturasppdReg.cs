using System;

namespace Facturacion.ENT
{
	public class ENTFacturasppdReg
	{


		#region Propiedades Públicas
		/// <summary>
		/// 
		/// </summary>
		public long IdFacturaPPD { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long IdFacturaCab { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long IdReservaCab { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long BookingId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public decimal MontoTotalFactura { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public decimal MontoPagado { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public decimal MontoSaldo { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public decimal MontoNC { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public byte NumParcialidad { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
