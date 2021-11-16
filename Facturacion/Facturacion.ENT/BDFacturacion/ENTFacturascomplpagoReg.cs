using System;

namespace Facturacion.ENT
{
	public class ENTFacturascomplpagoReg
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
		public long IdFacturaCab { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaPago { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string FormaDePagoP { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string MonedaP { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public decimal Monto { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
