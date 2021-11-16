using System;

namespace Facturacion.ENT
{
	public class ENTOpcionesmenuCat
	{


		#region Propiedades Públicas
		/// <summary>
		/// Identificador del Menu
		/// </summary>
		public int IdMenuOpcion { get; set; }

		/// <summary>
		/// Identificador del Menu al cual pertenece esta opcion
		/// </summary>
		public int IdMenuPadre { get; set; }

		/// <summary>
		/// Texto que se mostrara en la opcion del menu
		/// </summary>
		public string Nombre { get; set; }

		/// <summary>
		/// Descripcion breve del uso del menu
		/// </summary>
		public string Descripcion { get; set; }

		/// <summary>
		/// Url de la pagina Web que se mostrara al dar clic en el menu
		/// </summary>
		public string UrlMenu { get; set; }

		/// <summary>
		/// Ruta donde se encuentra la imagen que se colocara a un lado de la opcion del menu
		/// </summary>
		public string UrlImagen { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public int Orden { get; set; }

		/// <summary>
		/// Bandera que indica si esta activo el menu
		/// </summary>
		public bool Activo { get; set; }

		/// <summary>
		/// Fecha y Hora en formato local en que se registro en BD
		/// </summary>
		public DateTime FechaHoraLocal { get; set; }


		#endregion Propiedades Públicas

	}
}
