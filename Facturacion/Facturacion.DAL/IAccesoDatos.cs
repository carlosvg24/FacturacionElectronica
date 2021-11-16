using System;
using System.Data;

namespace Facturacion.DAL
{
	public interface IAccesoDatos
	{
		void IniciarPropiedades();
		void Agregar();
		void Actualizar();
		void Deshacer();
		void Eliminar(Int32 id);
		//DataTable Recuperar();
		//DataTable Recuperar(Int32 id);
	}
}
