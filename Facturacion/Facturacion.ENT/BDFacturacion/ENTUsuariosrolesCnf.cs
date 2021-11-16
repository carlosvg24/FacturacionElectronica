using System;

namespace Facturacion.ENT
{
	public class ENTUsuariosrolesCnf
	{


		#region Propiedades Públicas
		/// <summary>
		/// Identificador del Rol
		/// </summary>
		public int IdRol { get; set; }

		/// <summary>
		/// Identificador del usuario
		/// </summary>
		public int IdUsuario { get; set; }

		/// <summary>
		/// Fecha en que inicia la vigencia de asignacion del rol
		/// </summary>
		public DateTime FechaIniVigencia { get; set; }

		/// <summary>
		/// Fecha en que finaliza la vigencia de asignacion del rol
		/// </summary>
		public DateTime FechaFinVigencia { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool Activo { get; set; }

		/// <summary>
		/// Fecha Hora en que se registro el vinculo entre el rol y el usuario
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
