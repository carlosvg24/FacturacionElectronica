using System;

namespace Facturacion.ENT
{
	public class ENTEmpresaCat
	{


		#region Propiedades Públicas
		/// <summary>
		/// Identificador de la empresa
		/// </summary>
		public byte IdEmpresa { get; set; }

		/// <summary>
		/// Id del regimen Fiscal
		/// </summary>
		public string IdRegimenFiscal { get; set; }

		/// <summary>
		/// RFC de la empresa emisora
		/// </summary>
		public string Rfc { get; set; }

		/// <summary>
		/// Razon Social de la empresa emisora
		/// </summary>
		public string RazonSocial { get; set; }

		/// <summary>
		/// Numero de Certificado .CER usado para el timbrado
		/// </summary>
		public string NoCertificado { get; set; }

		/// <summary>
		/// Codigo postal del lugar de expedicion
		/// </summary>
		public string LugarExpedicion { get; set; }

		/// <summary>
		/// Bandera de eliminacion logica, 0. Inactivo. 1.Activo
		/// </summary>
		public bool Activo { get; set; }

		/// <summary>
		/// Usuario que realizo el registro
		/// </summary>
		public int IdUsuarioRegistro { get; set; }

		/// <summary>
		/// Fecha Hora en que se realizo el registro
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
