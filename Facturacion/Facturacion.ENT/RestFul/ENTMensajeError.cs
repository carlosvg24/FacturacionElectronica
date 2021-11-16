using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.RestFul
{
    public class RespuestaRESTFactura
    {
        private long _codigo;
        private string _mensaje;
        private string _pagosFacturados;

        public long Codigo {
            get { return _codigo; }
            set { _codigo = value; }
        }
        public string Mensaje {
            get { return _mensaje; }
            set { _mensaje = value; }
        }
        public string PagosFacturados {
            get { return _pagosFacturados; }
            set { _pagosFacturados = value; }
        }

        public RespuestaRESTFactura(long codigo, string mensaje, string pagosFacturados)
        {
            this._codigo = codigo;
            this._mensaje = mensaje;
            this._pagosFacturados = pagosFacturados;
        }
    }
}
