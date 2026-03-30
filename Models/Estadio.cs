namespace MiProyectoCSharp.Models
{
    /// <summary>
    /// Representa un estadio donde se disputarán los partidos.
    /// </summary>
    public class Estadio
    {
        /// <summary>
        /// Identificador único del estadio.
        /// </summary>
        public int IdEstadio { get; set; }

        /// <summary>
        /// Nombre oficial del estadio.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Capacidad máxima de espectadores que soporta el estadio.
        /// </summary>
        public int Capacidad { get; set; }

        /// <summary>
        /// Identificador de la ciudad donde se ubica el estadio.
        /// </summary>
        public int IdCiudad { get; set; }
    }
}
