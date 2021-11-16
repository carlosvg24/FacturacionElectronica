using System;

namespace Facturacion.ENT
{
	public class ENTSolicitudesfacDet
	{


		#region Propiedades Públicas
		/// <summary>
		/// 
		/// </summary>
		public long IdSolicitudesFac { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public int IdPeticionPago { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long PaymentId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string TipoPeticion { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool EsCorrecto { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Mensaje { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
