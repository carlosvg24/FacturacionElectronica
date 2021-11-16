using System;

namespace Facturacion.ENT
{
	public class ENTPagosomitidosglobalporiva
	{


		#region Propiedades Públicas
		/// <summary>
		/// 
		/// </summary>
		public long IdPagosCab { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaFacturaGlobal { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public decimal BaseIVA { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public decimal IVANavitaire { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public decimal IVAReal { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
