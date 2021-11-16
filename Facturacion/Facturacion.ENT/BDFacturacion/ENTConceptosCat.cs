using System;

namespace Facturacion.ENT
{
	public class ENTConceptosCat
	{


		#region Propiedades Públicas
		/// <summary>
		/// Identificador del Concepto usado para facturar
		/// </summary>
		public int IdConcepto { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string TipoComprobante { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string ClaveProdServ { get; set; }

		/// <summary>
		/// SKU o codigo utilizado para identificar el concepto
		/// </summary>
		public string NoIdentificacion { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string ClaveUnidad { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Unidad { get; set; }

		/// <summary>
		/// Descripcion del Concepto que se muestra en la factura
		/// </summary>
		public string Descripcion { get; set; }

		/// <summary>
		/// Orden en que se muestra en la factura el concepto

		/// </summary>
		public int OrdenConcepto { get; set; }

		/// <summary>
		/// Codigo con el que esta clasificado el tipo de Fee
		/// </summary>
		public byte ClasFact { get; set; }

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
