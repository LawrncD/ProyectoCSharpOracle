using System;

namespace MiProyectoCSharp.Models
{
    public class DirectorTecnico
    {
        public int IdDt { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Nacionalidad { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public int IdEquipo { get; set; } // Relación 1:1 requerida
    }
}
