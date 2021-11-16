using System;

namespace Facturacion.ENT
{
	public class ENTOrganppdCat
	{


		#region Propiedades Públicas
		/// <summary>
		/// 
		/// </summary>
		public int IdOrganPPD { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string OrganizationCode { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string OrganizationName { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool EsFacturaPPD { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool EsFacturaPagos { get; set; }

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
