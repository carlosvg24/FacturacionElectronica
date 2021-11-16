using System;

namespace Facturacion.ENT
{
	public class ENTCfdirelacionadosDet
	{


		#region Propiedades Públicas
		/// <summary>
		/// 
		/// </summary>
		public long IdCfdiRel { get; set; }

		/// <summary>
		/// Id de la Factura o Id Nota de Credito a la que se agregan los vinculos con otro comprobante
		/// </summary>
		public long IdCFDI { get; set; }

		/// <summary>
		/// Identificador de la factura o Nota de Credito que se esta vinculando
		/// </summary>
		public long IdCFDIVinculado { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string UUIDVinculado { get; set; }

		/// <summary>
		/// Cat SAT (c_TipoDeComprobante)   Tipo de comprobante  del CFDI al que se van a agregar los vinculos
		/// </summary>
		public string TipoComprobante { get; set; }

		/// <summary>
		/// Cat SAT (c_TipoRelacion). Atributo requerido para indicar la clave de la relación que existe entre éste que se está generando y el o los CFDI indicados con su UUID
		/// </summary>
		public string TipoRelacion { get; set; }

		/// <summary>
		/// Fecha hora del registro de la factura en formato de hora local
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
