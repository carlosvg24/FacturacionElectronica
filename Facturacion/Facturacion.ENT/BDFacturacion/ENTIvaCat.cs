using System;

namespace Facturacion.ENT
{
	public class ENTIvaCat
	{


		#region Propiedades Públicas
		/// <summary>
		/// 
		/// </summary>
		public byte IdPorIVA { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public decimal PorcIVA { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public decimal PorcTransformacion { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool BloquearFacturacion { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool Activo { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
