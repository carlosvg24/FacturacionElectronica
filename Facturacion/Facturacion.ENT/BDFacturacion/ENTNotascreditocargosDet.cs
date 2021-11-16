using System;

namespace Facturacion.ENT
{
	public class ENTNotascreditocargosDet
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
		/// Codigo DisplayCode asignado por Navitaire para identificar los FeeCode correspondientes a cargos Aeroportuarios
		/// </summary>
		public string CodigoCargo { get; set; }

		/// <summary>
		/// Monto del cargo aeroportuario
		/// </summary>
		public decimal Importe { get; set; }

		/// <summary>
		/// Fecha hora del registro de la Nota de Credito en formato de hora local
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
