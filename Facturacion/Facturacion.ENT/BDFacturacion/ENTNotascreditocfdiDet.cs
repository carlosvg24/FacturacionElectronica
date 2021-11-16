using System;

namespace Facturacion.ENT
{
	public class ENTNotascreditocfdiDet
	{


		#region Propiedades Públicas
		/// <summary>
		/// Identificador del cabecero de la Nota de Credito
		/// </summary>
		public long IdNotaCreditoCab { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string TransaccionID { get; set; }

		/// <summary>
		/// CFDI emitido por Pegaso despues de timbrar la NC
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
		/// Fecha hora del registro en formato de hora local
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
