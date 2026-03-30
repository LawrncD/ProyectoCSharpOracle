using System;

namespace MiProyectoCSharp.Models
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
        /// Identificador del equipo catalogado como local.
        /// </summary>
        public int IdEquipoLocal { get; set; }

        /// <summary>
        /// Identificador del equipo catalogado como visitante.
        /// </summary>
        public int IdEquipoVisitante { get; set; }

        /// <summary>
        /// Fecha y hora en la que se disputa el partido.
        /// </summary>
        public DateTime FechaHora { get; set; }

        /// <summary>
        /// Identificador del estadio donde se lleva a cabo el evento.
        /// </summary>
        public int IdEstadio { get; set; }

        /// <summary>
        /// Identificador del grupo al que pertenece este partido (si aplica para fase de grupos).
        /// </summary>
        public int IdGrupo { get; set; }

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
