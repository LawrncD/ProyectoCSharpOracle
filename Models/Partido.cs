using System;

namespace MiProyectoCSharp.Models
{
    public class Partido
    {
        public int IdPartido { get; set; }
        public int IdEquipoLocal { get; set; }
        public int IdEquipoVisitante { get; set; }
        public DateTime FechaHora { get; set; }
        public int IdEstadio { get; set; }
        public int IdGrupo { get; set; }
        public int GolesLocal { get; set; }
        public int GolesVisitante { get; set; }
    }
}
