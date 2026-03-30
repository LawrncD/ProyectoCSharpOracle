namespace MiProyectoCSharp.Models
{
    /// <summary>
    /// Representa al país anfitrión que organiza el torneo.
    /// </summary>
    public class PaisAnfitrion
    {
        /// <summary>
        /// Identificador único del país anfitrión.
        /// </summary>
        public int IdPaisAnfitrion { get; set; }

        /// <summary>
        /// Nombre del país anfitrión.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;
    }
}
