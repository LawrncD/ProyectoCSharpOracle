using System;

namespace MiProyectoCSharp.Models
{
    /// <summary>
    /// Representa al director técnico (entrenador) de un equipo.
    /// </summary>
    public class DirectorTecnico
    {
        /// <summary>
        /// Identificador único del director técnico.
        /// </summary>
        public int IdDt { get; set; }

        /// <summary>
        /// Nombre completo del director técnico.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Nacionalidad del director técnico.
        /// </summary>
        public string Nacionalidad { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de nacimiento del director técnico.
        /// </summary>
        public DateTime FechaNacimiento { get; set; }

        /// <summary>
        /// Identificador del equipo que dirige actualmente.
        /// </summary>
        public int IdEquipo { get; set; }
    }
}
