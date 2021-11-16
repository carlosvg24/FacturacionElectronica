using System;

namespace Facturacion.ENT
{
	public class ENTFacturascargosDet
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
		/// Codigo DisplayCode asignado por Navitaire para identificar los FeeCode correspondientes a cargos Aeroportuarios
		/// </summary>
		public string CodigoCargo { get; set; }

		/// <summary>
		/// Monto del cargo aeroportuario
		/// </summary>
		public decimal Importe { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool EsTua { get; set; }

		/// <summary>
		/// Fecha hora del registro de la factura en formato de hora local
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
