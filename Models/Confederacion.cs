namespace MiProyectoCSharp.Models
{
    /// <summary>
    /// Representa una confederación de fútbol a nivel continental o internacional.
    /// </summary>
    public class Confederacion
    {
        /// <summary>
        /// Identificador único de la confederación.
        /// </summary>
        public int IdConfederacion { get; set; }

        /// <summary>
        /// Nombre completo de la confederación.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Siglas representativas de la confederación.
        /// </summary>
        public string Siglas { get; set; } = string.Empty;
    }
}
