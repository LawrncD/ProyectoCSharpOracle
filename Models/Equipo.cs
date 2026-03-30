namespace MiProyectoCSharp.Models
{
    /// <summary>
    /// Representa un equipo de fútbol que participa en el torneo.
    /// </summary>
    public class Equipo
    {
        /// <summary>
        /// Identificador único del equipo.
        /// </summary>
        public int IdEquipo { get; set; }

        /// <summary>
        /// Nombre del equipo o selección.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// País al que representa el equipo.
        /// </summary>
        public string Pais { get; set; } = string.Empty;

        /// <summary>
        /// Identificador de la confederación a la cual se encuentra afiliado el equipo.
        /// </summary>
        public int IdConfederacion { get; set; }

        /// <summary>
        /// Valor total estipulado en el mercado para el equipo en general.
        /// </summary>
        public decimal ValorTotalEquipo { get; set; }
    }
}
