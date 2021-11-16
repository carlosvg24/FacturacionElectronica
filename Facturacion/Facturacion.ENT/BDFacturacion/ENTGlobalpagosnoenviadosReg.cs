using System;

namespace Facturacion.ENT
{
	public class ENTGlobalpagosnoenviadosReg
	{


		#region Propiedades Públicas
		/// <summary>
		/// 
		/// </summary>
		public int IdPagoOmitidoFG { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long BookingID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long PaymentID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string RecordLocator { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaPago { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string CodigoMoneda { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string LugarExpedicion { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaEnvioFG { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string MotivoOmision { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
