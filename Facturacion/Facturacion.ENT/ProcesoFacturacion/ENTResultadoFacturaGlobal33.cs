using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.ProcesoFacturacion
{
    public class ENTResultadoFacturaGlobal33
    {
        public DateTime FechaPagos { get; set; }

        public string Moneda { get; set; }
        public string CodigoPostal { get; set; }
        public bool EsFrontera { get; set; }
        public int NumPagosInicial { get; set; }
        public int NumPagosEnviadosFG { get; set; }
        public int NumPagosOmitidos { get; set; }
        public int NumFacturasGeneradas { get; set; }
        public string Resultado { get; set; }
        public List<ENTEnvioFacturaGlobal>  ListaEnviosFG { get; set; }
        public List<ENTGlobalpagosnoenviadosReg> ListaPagosNoEnviados { get; set; }
    }

   
}
