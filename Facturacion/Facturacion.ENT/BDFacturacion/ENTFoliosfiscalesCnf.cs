using System;

namespace Facturacion.ENT
{
	public class ENTFoliosfiscalesCnf
	{


		#region Propiedades Públicas
		/// <summary>
		/// Identificador del Folio Fiscal
		/// </summary>
		public int IdFolioFiscal { get; set; }

		/// <summary>
		/// Id del tipo de comprobante, FA(Factura),NC(NotaCredito),FG(FacturaGlobal)
		/// </summary>
		public string TipoComprobante { get; set; }

		/// <summary>
		/// Clave utilizada por Pegaso para realizar el timbrado. Ej. clave="FAC" 
		/// </summary>
		public string ClavePegaso { get; set; }

		/// <summary>
		/// Nombre utilizado por pegaso para realizar el timbrado. Ej. nombre="Factura B" 
		/// </summary>
		public string NombrePegaso { get; set; }

		/// <summary>
		/// Serie utilizada en el tipo de comprobante
		/// </summary>
		public string Serie { get; set; }

		/// <summary>
		/// Folio con que inicia la emision del comprobante
		/// </summary>
		public long FolioInicial { get; set; }

		/// <summary>
		/// Folio limite que se emitira para este folio
		/// </summary>
		public long FolioFinal { get; set; }

		/// <summary>
		/// Ultimo folio utilizado para el comprobante
		/// </summary>
		public long FolioActual { get; set; }

		/// <summary>
		/// Fecha de fin de vigencia del folio fiscal
		/// </summary>
		public DateTime FechaFinVigencia { get; set; }

		/// <summary>
		/// Bandera de eliminacion logica, 0. Inactivo. 1.Activo
		/// </summary>
		public bool Activo { get; set; }

		/// <summary>
		/// Usuario que realizó el registro
		/// </summary>
		public int UsuarioRegistro { get; set; }

		/// <summary>
		/// Fecha Hora en que se realizo el registro
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
