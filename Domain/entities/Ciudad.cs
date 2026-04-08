
using System;

namespace MiProyectoCSharp.Domain.Entities
{
    /// <summary>
    /// Representa una ciudad anfitriona donde se alojarán los estadios.
    /// </summary>
    public class Ciudad
    {
        /// <summary>
        /// Identificador único de la ciudad.
        /// </summary>
        public int IdCiudad { get; set; }

        /// <summary>
        /// Nombre de la ciudad.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// País anfitrión al cual pertenece esta ciudad.
        /// </summary>
        public PaisAnfitrion PaisAnfitrion { get; set; }
    }
}
