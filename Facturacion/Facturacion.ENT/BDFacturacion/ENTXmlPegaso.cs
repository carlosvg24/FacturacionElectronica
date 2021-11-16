using System;

namespace Facturacion.ENT
{
	public class ENTXmlPegaso
	{


		#region Propiedades Públicas
		/// <summary>
		/// Identificador de la peticion realizada a pegaso para timbrar
		/// </summary>
		public long IdPeticionPAC { get; set; }

		/// <summary>
		/// Fecha en que se realizo la peticion a Pegaso
		/// </summary>
		public DateTime FechaPeticion { get; set; }

		/// <summary>
		/// Identificador del comprobante enviado
		/// </summary>
		public long FolioCFDI { get; set; }

		/// <summary>
		/// Indica el tipo de comprobante que se trata
		/// </summary>
		public string TipoComprobante { get; set; }

		/// <summary>
		/// XML de request enviado para solicitar el timbrado
		/// </summary>
		public string XMLRequest { get; set; }

		/// <summary>
		/// XML de respuesta enviado por pegaso
		/// </summary>
		public string XMLResponse { get; set; }

		/// <summary>
		/// Mensaje de respuesta a la peticion de pegaso
		/// </summary>
		public string MensajeRespuesta { get; set; }

		/// <summary>
		/// Bandera que indica si la peticion fue correcta
		/// </summary>
		public bool EsCorrecto { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaTimbrado { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Transaccion_Id { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Transaccion_Tipo { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Transaccion_Estatus { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string CFD_CadenaOriginal { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string CFD_NoCertificado { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string CFD_ComprobanteStr { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string CFD_Serie { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string CFD_CodigoDeBarras { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime CFD_Fecha { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long CFD_Folio { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string CFD_Sello { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string TFD_UUID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime TFD_FechaTimbrado { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string TFD_NoCertificadoSAT { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string TFD_SelloSAT { get; set; }

		/// <summary>
		/// Fecha en que se registro en BD Facturacion
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string PAC { get; set; }


		#endregion Propiedades Públicas

	}
}
