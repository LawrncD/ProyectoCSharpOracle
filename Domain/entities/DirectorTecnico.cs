using System;

namespace MiProyectoCSharp.Domain.Entities
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
        /// Equipo que dirige actualmente.
        /// </summary>
        public Equipo Equipo { get; set; }
    }
}
