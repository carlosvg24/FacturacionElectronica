using System;

namespace Facturacion.ENT
{
	public class ENTLocationCat
	{


		#region Propiedades Públicas
		/// <summary>
		/// 
		/// </summary>
		public int IdLocation { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string LocationCode { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string LugarExpedicion { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool Activo { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool EsFranjaFronteriza { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string LugarExpPublicoGeneral { get; set; }


		#endregion Propiedades Públicas

	}
}
