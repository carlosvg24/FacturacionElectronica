using System;

namespace Facturacion.ENT
{
	public class ENTLog
	{


		#region Propiedades Públicas
		/// <summary>
		/// Identificador del error 
		/// </summary>
		public long IdLog { get; set; }

		/// <summary>
		/// Indica el modulo en que se genero el error
		/// </summary>
		public string Modulo { get; set; }

		/// <summary>
		/// BookingId del PNR en que se genero el error, para temas de rastreo de errores por reservacion
		/// </summary>
		public string PNR { get; set; }

		/// <summary>
		/// Indica el tipo de error generado, BD, Proceso,Pantalla,WS, etc
		/// </summary>
		public string TipoError { get; set; }

		/// <summary>
		/// Nivel de importancia o criticidad asignado
		/// </summary>
		public byte NivelImportancia { get; set; }

		/// <summary>
		/// Mensaje de la clase Exception
		/// </summary>
		public string Descripcion { get; set; }

		/// <summary>
		/// Fecha en que se registro en BD Facturacion
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
