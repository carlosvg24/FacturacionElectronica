using System;

namespace Facturacion.ENT
{
	public class ENTParametrosCnf
	{


		#region Propiedades Públicas
		/// <summary>
		/// Identificador del Parametro
		/// </summary>
		public int IdParametro { get; set; }

		/// <summary>
		/// Nombre del parametro
		/// </summary>
		public string Nombre { get; set; }

		/// <summary>
		/// Descripcion del parametro
		/// </summary>
		public string Descripcion { get; set; }

		/// <summary>
		/// Valor asignado al parametro
		/// </summary>
		public string Valor { get; set; }

		/// <summary>
		/// Bandera que indica si el parametro esta activo
		/// </summary>
		public bool Activo { get; set; }

		/// <summary>
		/// Fecha y Hora en formato local en que se registro en BD
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
