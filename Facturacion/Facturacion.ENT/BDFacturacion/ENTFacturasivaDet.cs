using System;

namespace Facturacion.ENT
{
	public class ENTFacturasivaDet
	{


		#region Propiedades Públicas
		/// <summary>
		/// Identificador del cabecero de la factura
		/// </summary>
		public long IdFacturaCab { get; set; }

		/// <summary>
		/// Identificador del detalle de la factura
		/// </summary>
		public int IdFacturaDet { get; set; }

		/// <summary>
		/// Cat SAT (c_TipoFactor) Atributo requerido para señalar la clave del tipo de factor que se aplica a la base del impuesto.
		/// </summary>
		public string TipoFactor { get; set; }

		/// <summary>
		/// Atributo condicional para señalar el valor de la tasa o cuota del impuesto que se traslada para el presente concepto. Es requerido cuando el atributo TipoFactor tenga una clave que corresponda a Tasa o Cuota.
		/// </summary>
		public decimal TasaOCuota { get; set; }

		/// <summary>
		/// Atributo requerido para señalar la base para el cálculo del impuesto, la determinación de la base se realiza de acuerdo con las disposiciones fiscales vigentes. No se permiten valores negativos.
		/// </summary>
		public decimal Base { get; set; }

		/// <summary>
		/// Cat SAT (c_Impuesto). Atributo requerido para señalar la clave del tipo de impuesto trasladado aplicable al concepto.
		/// </summary>
		public string Impuesto { get; set; }

		/// <summary>
		/// Atributo condicional para señalar el importe del impuesto trasladado que aplica al concepto. No se permiten valores negativos. Es requerido cuando TipoFactor sea Tasa o Cuota
		/// </summary>
		public decimal Importe { get; set; }

		/// <summary>
		/// Fecha hora del registro de la factura en formato de hora local
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
