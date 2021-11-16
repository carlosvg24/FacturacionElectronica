using System;

namespace Facturacion.ENT
{
	public class ENTIvaporcpCat
	{


		#region Propiedades Públicas
		/// <summary>
		/// 
		/// </summary>
		public int IdIvaPorCP { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string CodigoPostal { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public int PorcIVA { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool EsFrontera { get; set; }

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
