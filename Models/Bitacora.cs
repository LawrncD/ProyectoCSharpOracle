using System;

namespace MiProyectoCSharp.Models
{
    public class Bitacora
    {
        public int IdRegistro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaHoraIngreso { get; set; }
        public DateTime? FechaHoraSalida { get; set; } // Puede ser null mientras esté logueado
    }
}
