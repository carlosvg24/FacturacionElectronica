using System;

namespace Facturacion.ENT
{
	public class ENTFacturascomplpagdoctoRel
	{


		#region Propiedades Públicas
		/// <summary>
		/// 
		/// </summary>
		public long IdFacturaComplPago { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long IdPagosCab { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long IdFacturaPPD { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string IdDocumento { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Serie { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long Folio { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string MonedaDR { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string MetodoDePagoDR { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public byte NumParcialidad { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public decimal ImpSaldoAnt { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public decimal ImpPagado { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public decimal ImpSaldoInsoluto { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
