using System;

namespace Facturacion.ENT
{
	public class ENTRolesCat
	{


		#region Propiedades Públicas
		/// <summary>
		/// Identificador del Rol
		/// </summary>
		public int IdRol { get; set; }

		/// <summary>
		/// Nombre del Rol
		/// </summary>
		public string Nombre { get; set; }

		/// <summary>
		/// Descripcion del Rol
		/// </summary>
		public string DescripcionRol { get; set; }

		/// <summary>
		/// Bandera que indica si el rol esta activo
		/// </summary>
		public bool Activo { get; set; }

		/// <summary>
		/// Fecha Hora en que se registro el rol
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
