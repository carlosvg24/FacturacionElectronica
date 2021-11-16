using System;

namespace Facturacion.ENT
{
	public class ENTFacturascfdiDet
	{


		#region Propiedades Públicas
		/// <summary>
		/// Identificador del cabecero de la factura
		/// </summary>
		public long IdFacturaCab { get; set; }

		/// <summary>
		/// Cadena emitida por Pegaso al timbrar la factura
		/// </summary>
		public string TransaccionID { get; set; }

		/// <summary>
		/// CFDI emitido por Pegaso despues de timbrar la factura
		/// </summary>
		public string CFDI { get; set; }

		/// <summary>
		/// Sello Digital del emisor
		/// </summary>
		public string CadenaOriginal { get; set; }

		/// <summary>
		/// Sello Digital del SAT
		/// </summary>
		public DateTime FechaTimbrado { get; set; }

		/// <summary>
		/// Fecha hora del registro de la factura en formato de hora local
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
