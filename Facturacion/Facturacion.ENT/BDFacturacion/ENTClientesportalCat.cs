using System;

namespace Facturacion.ENT
{
	public class ENTClientesportalCat
	{


		#region Propiedades Públicas
		/// <summary>
		/// 
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Contrasenia { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string RFC { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string TAXID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Nombre { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string UsoCFDI { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string FormaPago { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Pais { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public int ClienteTipoId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Guid CodigoVerificacion { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool UsuarioVerificado { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool UsuarioActivo { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
