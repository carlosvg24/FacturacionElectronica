using System;

namespace Facturacion.ENT
{
	public class ENTXmlFtpReg
	{


		#region Propiedades Públicas
		/// <summary>
		/// 
		/// </summary>
		public long IdXMLFTP { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long IdPeticionPAC { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaTimbrado { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string RutaCFDI { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string RutaPDF { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
