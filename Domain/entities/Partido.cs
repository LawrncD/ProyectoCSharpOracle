using System;

namespace MiProyectoCSharp.Domain.Entities
{
    /// <summary>
    /// Representa un partido de fútbol disputado entre dos equipos.
    /// </summary>
    public class Partido
    {
        /// <summary>
        /// Identificador del partido.
        /// </summary>
        public int IdPartido { get; set; }

        /// <summary>
        /// Equipo catalogado como local.
        /// </summary>
        public Equipo EquipoLocal { get; set; }

        /// <summary>
        /// Equipo catalogado como visitante.
        /// </summary>
        public Equipo EquipoVisitante { get; set; }

        /// <summary>
        /// Fecha y hora en la que se disputa el partido.
        /// </summary>
        public DateTime FechaHora { get; set; }

        /// <summary>
        /// Estadio donde se lleva a cabo el evento.
        /// </summary>
        public Estadio Estadio { get; set; }

        /// <summary>
        /// Grupo al que pertenece este partido (si aplica para fase de grupos).
        /// </summary>
        public Grupo Grupo { get; set; }

        /// <summary>
        /// Cantidad de goles anotados por el equipo local.
        /// </summary>
        public int GolesLocal { get; set; }

        /// <summary>
        /// Cantidad de goles anotados por el equipo visitante.
        /// </summary>
        public int GolesVisitante { get; set; }
    }
}
