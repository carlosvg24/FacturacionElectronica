using System;

namespace FinanzasTools
{
	public class ENTGendescripcionesCat
	{


		#region Propiedades Públicas
		/// <summary>
		/// 
		/// </summary>
		public int IdGenDescripcion { get; set; }

		/// <summary>
		/// Clave del catalogo general
		/// </summary>
		public string CveTabla { get; set; }

		/// <summary>
		/// Clave o codigo de la descripcion SAT
		/// </summary>
		public string CveValor { get; set; }

		/// <summary>
		/// Descripcion asignada por el SAT
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
