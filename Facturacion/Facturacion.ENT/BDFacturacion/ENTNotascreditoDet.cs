using System;

namespace Facturacion.ENT
{
	public class ENTNotascreditoDet
	{


		#region Propiedades Públicas
		/// <summary>
		/// Identificador del cabecero de la Nota de Credito
		/// </summary>
		public long IdNotaCreditoCab { get; set; }

		/// <summary>
		/// Identificador del detalle de la Nota de Credito
		/// </summary>
		public int IdNotaCreditoDet { get; set; }

		/// <summary>
		/// Cat SAT (c_ClaveProdServ). Atributo requerido para expresar la clave del producto o del servicio amparado por el presente concepto. Es requerido y deben utilizar las claves del catálogo de productos y servicios, cuando los conceptos que registren por sus actividades correspondan con dichos conceptos.
		/// </summary>
		public string ClaveProdServ { get; set; }

		/// <summary>
		/// Atributo opcional para expresar el número de parte, identificador del producto o del servicio, la clave de producto o servicio, SKU o equivalente, propia de la operación del emisor, amparado por el presente concepto
		/// </summary>
		public string NoIdentificacion { get; set; }

		/// <summary>
		/// Atributo requerido para precisar la cantidad de bienes o servicios del tipo particular definido por el presente concepto.
		/// </summary>
		public int Cantidad { get; set; }

		/// <summary>
		/// Cat SAT (c_ClaveUnidad). Atributo requerido para precisar la clave de unidad de medida estandarizada aplicable para la cantidad expresada en el concepto. La unidad debe corresponder con la descripción del concepto.
		/// </summary>
		public string ClaveUnidad { get; set; }

		/// <summary>
		/// Cat SAT (c_ClaveUnidad.Descripcion) Atributo opcional para precisar la unidad de medida propia de la operación del emisor, aplicable para la cantidad expresada en el concepto. La unidad debe corresponder con la descripción del concepto.
		/// </summary>
		public string Unidad { get; set; }

		/// <summary>
		/// Cat Viva (VBFac_Conceptos_Cat). Atributo requerido para precisar la descripción del bien o servicio cubierto por el presente concepto.
		/// </summary>
		public string Descripcion { get; set; }

		/// <summary>
		/// Atributo requerido para precisar el valor o precio unitario del bien o servicio cubierto por el presente concepto.
		/// </summary>
		public decimal ValorUnitario { get; set; }

		/// <summary>
		/// Atributo requerido para precisar el importe total de los bienes o servicios del presente concepto. Debe ser equivalente al resultado de multiplicar la cantidad por el valor unitario expresado en el concepto. No se permiten valores negativos.
		/// </summary>
		public decimal Importe { get; set; }

		/// <summary>
		/// Atributo opcional para representar el importe de los descuentos aplicables al concepto. No se permiten valores negativos.
		/// </summary>
		public decimal Descuento { get; set; }

		/// <summary>
		/// Fecha hora del registro de la NC en formato de hora local
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
