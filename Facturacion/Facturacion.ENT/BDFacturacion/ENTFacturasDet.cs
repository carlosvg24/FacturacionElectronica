using System;

namespace Facturacion.ENT
{
    public class ENTFacturasDet
    {


        #region Propiedades Públicas
        /// <summary>
        /// 
        /// </summary>
        public long IdFacturaCab { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int IdFacturaDet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClaveProdServ { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NoIdentificacion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Cantidad { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClaveUnidad { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Unidad { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal ValorUnitario { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal Importe { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal Descuento { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime FechaHoraLocal { get; set; }

        #region Nuevas Propiedades para Terceros
        /// <summary>
        /// RFC del proveedor de Hotel
        /// </summary>
        public string RFC { get; set; }
        /// <summary>
        /// Razon Social del proveedor de Hotel
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Calle - Domicilio fiscal del proveedor de Hotel
        /// </summary>
        public string Calle { get; set; }
        /// <summary>
        /// No, - Domicilio fiscal del proveedor de Hotel
        /// </summary>
        public string NumExt { get; set; }
        /// <summary>
        /// Municipio - Domicilio fiscal del proveedor de Hotel
        /// </summary>
        public string Municipio { get; set; }
        /// <summary>
        /// Estado - Domicilio fiscal del proveedor de Hotel
        /// </summary>
        public string Estado { get; set; }
        /// <summary>
        /// País - Domicilio fiscal del proveedor de Hotel
        /// </summary>
        public string Pais { get; set; }
        /// <summary>
        /// CP - Domicilio fiscal del proveedor de Hotel
        /// </summary>
        public string CodigoPostal { get; set; }
        #endregion
        #endregion Propiedades Públicas

    }
}
