using System;

namespace Facturacion.ENT
{
	public class ENTGlobalcargosaeroDet
	{


		#region Propiedades Públicas
		/// <summary>
		/// 
		/// </summary>
		public long IdFacturaCab { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long IdFacturaDet { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long PaymentID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string CodigoCargo { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public decimal Importe { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool EsTua { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
