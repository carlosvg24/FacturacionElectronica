using System;

namespace Facturacion.ENT
{
	public class ENTGlobalticketsDet
	{


		#region Propiedades Públicas
		/// <summary>
		/// Identificador del cabecero de la factura Global
		/// </summary>
		public long IdFacturaCab { get; set; }

		/// <summary>
		/// Identificador del detalle de la factura Global
		/// </summary>
		public int IdFacturaDet { get; set; }

		/// <summary>
		/// Identificador del Booking sobre el que se genera la factura
		/// </summary>
		public long BookingID { get; set; }

		/// <summary>
		/// Identificador del Pago o Ticket registrado en la factura Global
		/// </summary>
		public long PaymentID { get; set; }

		/// <summary>
		/// Identificador de la NC con la que se ajusto la Global
		/// </summary>
		public long IdNotaCredito { get; set; }

		/// <summary>
		/// Identificador de la Factura creada al cliente
		/// </summary>
		public long IdFacturaCabCliente { get; set; }

		/// <summary>
		/// Fecha hora del registro de la factura en formato de hora local
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public decimal MontoTarifa { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public decimal MontoServAdic { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public decimal MontoTUA { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public decimal MontoOtrosCargos { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public decimal MontoIVA { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public decimal MontoTotal { get; set; }


		#endregion Propiedades Públicas

	}
}
