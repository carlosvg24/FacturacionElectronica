using System;

namespace Facturacion.ENT
{
	public class ENTDistribucionpagos
	{


		#region Propiedades Públicas
		/// <summary>
		/// 
		/// </summary>
		public long IdDistribucionPagos { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long BookingID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string RecordLocator { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime CreatedDate { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime ModifiedDate { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaProcesamiento { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool ProcesoExitoso { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool ConDescartePorDiferencia { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string MensajeError { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
