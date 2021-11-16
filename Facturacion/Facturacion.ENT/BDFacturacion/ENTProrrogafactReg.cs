using System;

namespace Facturacion.ENT
{
	public class ENTProrrogafactReg
	{


		#region Propiedades Públicas
		/// <summary>
		/// 
		/// </summary>
		public int IdProrroga { get; set; }

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
		public int NumDiasFacturacion { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool Activo { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public int IdUsuario { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
