using System;

namespace Facturacion.ENT
{
	public class ENTGencatalogosCat
	{


		#region Propiedades Públicas
		/// <summary>
		/// 
		/// </summary>
		public int IdGenCatalogo { get; set; }

		/// <summary>
		/// Clave del catalogo Sat
		/// </summary>
		public string CveTabla { get; set; }

		/// <summary>
		/// Nombre del Catalogo SAT
		/// </summary>
		public string Nombre { get; set; }

		/// <summary>
		/// Descripcion de la informacion que contiene
		/// </summary>
		public string Descripcion { get; set; }

		/// <summary>
		/// Bandera de eliminacion logica, 0. Inactivo. 1.Activo
		/// </summary>
		public bool Activo { get; set; }

		/// <summary>
		/// Usuario que realizó el registro
		/// </summary>
		public int UsuarioRegistro { get; set; }

		/// <summary>
		/// Fecha Hora en que se realizo el registro
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
