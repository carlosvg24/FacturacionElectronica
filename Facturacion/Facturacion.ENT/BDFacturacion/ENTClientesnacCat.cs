using System;

namespace Facturacion.ENT
{
	public class ENTClientesnacCat
	{


		#region Propiedades Públicas
		/// <summary>
		/// Identificador del cliente
		/// </summary>
		public int IdCliente { get; set; }

		/// <summary>
		/// RFC con el que factura el cliente o receptor
		/// </summary>
		public string RFC { get; set; }

		/// <summary>
		/// Razon social del cliente receptor
		/// </summary>
		public string Nombre { get; set; }

		/// <summary>
		/// Atributo requerido para expresar la clave del uso que dará a esta factura el receptor del CFDI. Catalogo SAT (c_UsoCFDI)
		/// </summary>
		public string UsoCFDI { get; set; }

		/// <summary>
		/// Fecha Hora en que se realizo el registro
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
