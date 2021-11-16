using System;

namespace Facturacion.ENT
{
	public class ENTAgentesCat
	{


		#region Propiedades Públicas
		/// <summary>
		/// Identificador del Agente en Navitaire
		/// </summary>
		public int IdAgente { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long AgentID { get; set; }

		/// <summary>
		/// Codigo del Agente
		/// </summary>
		public string CodigoAgente { get; set; }

		/// <summary>
		/// Codigo asignado en SAP
		/// </summary>
		public string CodigoSAP { get; set; }

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
