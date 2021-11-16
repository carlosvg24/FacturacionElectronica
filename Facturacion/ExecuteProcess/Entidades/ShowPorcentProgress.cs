using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process.Entidades
{
    public class ShowPorcentProgressEventArgs : EventArgs
    {
        public int OldPorcent { get; set; }

        public int NewPorcent { get; set; }

        public string Message { get; set; }

        public ShowPorcentProgressEventArgs(int porcentajeAnterior, int porcentajeNuevo, string mensaje)
        {
            this.OldPorcent = porcentajeAnterior;
            this.NewPorcent = porcentajeNuevo;
            this.Message = mensaje;
        }
    }
}
