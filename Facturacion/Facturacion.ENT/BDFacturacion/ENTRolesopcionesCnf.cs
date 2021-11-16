using System;

namespace Facturacion.ENT
{
	public class ENTRolesopcionesCnf
	{


		#region Propiedades Públicas
		/// <summary>
		/// Identificador del Rol
		/// </summary>
		public int IdRol { get; set; }

		/// <summary>
		/// Identificador del Menu
		/// </summary>
		public int IdMenuOpcion { get; set; }

		/// <summary>
		/// Bandera que indica si el rol tiene permisos para agregar
		/// </summary>
		public bool PermisoAgregar { get; set; }

		/// <summary>
		/// Bandera que indica si el rol tiene permisos para Editar
		/// </summary>
		public bool PermisoEditar { get; set; }

		/// <summary>
		/// Bandera que indica si el rol tiene permisos para Eliminar
		/// </summary>
		public bool PermisoEliminar { get; set; }

		/// <summary>
		/// Bandera que indica si el rol tiene permisos para Consultar
		/// </summary>
		public bool PermisoConsultar { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool Activo { get; set; }

		/// <summary>
		/// Fecha y Hora en formato local en que se registro en BD
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
