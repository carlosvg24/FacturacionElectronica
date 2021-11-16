using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comun.Utils
{
    public static class Tipo
    {
        public static partial class ClientePortal
        {
            /*
             * SYS	Usuario interno del sistema
                CTE	Cliente de Viva
                OTA	Socio comercial
                EMP	Empleado de viva
            */
        public const String
            Sistema = "SYS",
            Cliente = "CTE",
            Socio = "OTA",
            Empleado = "EMP";
        }
            
    }
}
