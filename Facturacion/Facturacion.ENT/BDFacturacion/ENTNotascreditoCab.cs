using System;

namespace Facturacion.ENT
{
	public class ENTNotascreditoCab
	{


		#region Propiedades Públicas
		/// <summary>
		/// Identificador de la Nota de Credito
		/// </summary>
		public long IdNotaCreditoCab { get; set; }

		/// <summary>
		/// Identificador de la empresa (VBFac_Empresa_Cat)
		/// </summary>
		public byte IdEmpresa { get; set; }

		/// <summary>
		/// Identificador del Booking sobre el que se genera la factura
		/// </summary>
		public long BookingID { get; set; }

		/// <summary>
		/// FechaHora en que se realizo el timbrado de la Nota de Credito
		/// </summary>
		public DateTime FechaHoraExpedicion { get; set; }

		/// <summary>
		/// Identificador del tipo de Factura al que se genero la NC (FA, FG,FM)
		/// </summary>
		public string TipoFacturacion { get; set; }

		/// <summary>
		/// Version utilizada para enviar la solicitud a Pegaso. 3.3
		/// </summary>
		public string Version { get; set; }

		/// <summary>
		/// Serie utilizada para la Nota de Credito
		/// </summary>
		public string Serie { get; set; }

		/// <summary>
		/// Folio de la Nota de Credito, se recupera del catalogo de  foliosFiscales
		/// </summary>
		public long FolioFiscal { get; set; }

		/// <summary>
		/// Identificador SAT de la Nota de Credito generada
		/// </summary>
		public string UUID { get; set; }

		/// <summary>
		/// Numero de Transaccion armada para el envio
		/// </summary>
		public string TransactionID { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public long IdPeticionPAC { get; set; }

		/// <summary>
		/// Indica si la NC esta Activa o Cancelada, en caso de la cancelacion de la NC entonces se deben liberar los detalles de los conceptos de la venta
		/// </summary>
		public string Estatus { get; set; }

		/// <summary>
		/// Atributo requerido para precisar la Clave del Registro Federal de Contribuyentes correspondiente al contribuyente receptor del comprobante.
		/// </summary>
		public string RfcReceptor { get; set; }

		/// <summary>
		/// Correo electronico donde se enviara el PDF y CFDI cuando se timbre una factura
		/// </summary>
		public string EmailReceptor { get; set; }

		/// <summary>
		/// Bandera que indica si el receptor es extranjero
		/// </summary>
		public bool EsExtranjero { get; set; }

		/// <summary>
		/// Atributo condicional para registrar la clave del país de residencia para efectos fiscales del receptor del comprobante, cuando se trate de un extranjero, y que es conforme con la especificación ISO 3166-1 alpha-3. Es requerido cuando se incluya el complemento de comercio exterior o se registre el atributo NumRegIdTrib. Catalogo SAT (c_Pais)
		/// </summary>
		public string IdPaisResidenciaFiscal { get; set; }

		/// <summary>
		/// TaxId. Atributo condicional para expresar el número de registro de identidad fiscal del receptor cuando sea residente en el extranjero. Es requerido cuando se incluya el complemento de comercio exterior
		/// </summary>
		public string NumRegIdTrib { get; set; }

		/// <summary>
		/// Atributo requerido para expresar la clave del uso que dará a esta factura el receptor del CFDI. Catalogo SAT (c_UsoCFDI)
		/// </summary>
		public string UsoCFDI { get; set; }

		/// <summary>
		/// Identificador de la forma de pago (VBFac_FormaPago_Cat)
		/// </summary>
		public string FormaPago { get; set; }

		/// <summary>
		/// Codigo del metodo de Pago del catalogo SAT (c_MetodoPago)
		/// </summary>
		public string MetodoPago { get; set; }

		/// <summary>
		/// Identificador del Tipo de comprobante del catalogo SAT(c_TipoDeComprobante)
		/// </summary>
		public string TipoComprobante { get; set; }

		/// <summary>
		/// Atributo requerido para incorporar el código postal del lugar de expedición del comprobante (domicilio de la matriz o de la sucursal). catalogo SAT (c_CodigoPostal_Parte_1)
		/// </summary>
		public string LugarExpedicion { get; set; }

		/// <summary>
		/// Se utiliza para expresar las condiciones comerciales aplicables para el pago del comprobante fiscal digital 
		/// </summary>
		public string CondicionesPago { get; set; }

		/// <summary>
		/// Atributo requerido para identificar la clave de la moneda utilizada para expresar los montos, cuando se usa moneda nacional se registra MXN. Catalogo SAT(c_Moneda)
		/// </summary>
		public string Moneda { get; set; }

		/// <summary>
		/// Atributo condicional para representar el tipo de cambio conforme con la moneda usada. Es requerido cuando la clave de moneda es distinta de MXN y de XXX. El valor debe reflejar el número de pesos mexicanos que equivalen a una unidad de la divisa señalada en el atributo moneda. Si el valor está fuera del porcentaje aplicable a la moneda tomado del catálogo c_Moneda, el emisor debe obtener del PAC que vaya a timbrar el CFDI, de manera no automática, una clave de confirmación para ratificar que el valor es correcto e integrar dicha clave en el atributo Confirmacion.
		/// </summary>
		public decimal TipoCambio { get; set; }

		/// <summary>
		/// Requerido para representar la suma de los importes de los conceptos antes de descuentos e impuesto. No se permiten valores negativos.
		/// </summary>
		public decimal SubTotal { get; set; }

		/// <summary>
		/// Condicional para representar el importe total de los descuentos aplicables antes de impuestos. No se permiten valores negativos. Se debe registrar cuando existan conceptos con descuento
		/// </summary>
		public decimal Descuento { get; set; }

		/// <summary>
		/// Atributo requerido para representar la suma del subtotal, menos los descuentos aplicables, más las contribuciones recibidas (impuestos trasladados - federales o locales, derechos, productos, aprovechamientos, aportaciones de seguridad social, contribuciones de mejoras) menos los impuestos retenidos. Si el valor es superior al límite que establezca el SAT en la Resolución Miscelánea Fiscal vigente, el emisor debe obtener del PAC que vaya a timbrar el CFDI, de manera no automática, una clave de confirmación para ratificar que el valor es correcto e integrar dicha clave en el atributo Confirmacion. No se permiten valores negativos.
		/// </summary>
		public decimal Total { get; set; }

		/// <summary>
		/// Monto total de la NC por concepto de Tarifa
		/// </summary>
		public decimal MontoTarifa { get; set; }

		/// <summary>
		/// Monto Total de la NC por concepto de Servicios Adicionales
		/// </summary>
		public decimal MontoServAdic { get; set; }

		/// <summary>
		/// Monto Total  de la NC por concepto de TUA
		/// </summary>
		public decimal MontoTUA { get; set; }

		/// <summary>
		/// Monto total de la NC por concepto de Otros Cargos
		/// </summary>
		public decimal MontoOtrosCargos { get; set; }

		/// <summary>
		/// Monto total de la NC por concepto de IVA
		/// </summary>
		public decimal MontoIVA { get; set; }

		/// <summary>
		/// Id del agente que registro la NC 
		/// </summary>
		public int IdAgente { get; set; }

		/// <summary>
		/// Identificador del usuario que realizo el proceso de NC
		/// </summary>
		public int IdUsuario { get; set; }

		/// <summary>
		/// Fecha hora del registro de la NC en formato de hora local
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }

		/// <summary>
		/// Identificador del usuario que realizo la cancelacion de la NC
		/// </summary>
		public int IdUsuarioCancelo { get; set; }

		/// <summary>
		/// Fecha hora en que se cancelo la NC
		/// </summary>
		public DateTime FechaHoraCancelLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
