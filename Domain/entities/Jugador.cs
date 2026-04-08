using System;

namespace MiProyectoCSharp.Domain.Entities
{
    /// <summary>
    /// Representa un jugador que pertenece a un equipo.
    /// </summary>
    public class Jugador
    {
        /// <summary>
        /// Identificador único del jugador.
        /// </summary>
        public int IdJugador { get; set; }

        /// <summary>
        /// Nombre completo del jugador.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de nacimiento del jugador.
        /// </summary>
        public DateTime FechaNacimiento { get; set; }

        /// <summary>
        /// Posición principal en la que juega dentro del campo.
        /// </summary>
        public string? Posicion { get; set; }

        /// <summary>
        /// Peso corporal del jugador en kilogramos.
        /// </summary>
        public decimal Peso { get; set; }

        /// <summary>
        /// Estatura del jugador en metros.
        /// </summary>
        public decimal Estatura { get; set; }

        /// <summary>
        /// Valor estimado del jugador en el mercado.
        /// </summary>
        public decimal ValorMercado { get; set; }

        /// <summary>
        /// Equipo al cual pertenece el jugador.
        /// </summary>
        public Equipo Equipo { get; set; }
    }
}
