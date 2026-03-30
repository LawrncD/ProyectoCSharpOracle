namespace MiProyectoCSharp.Models
{
    /// <summary>
    /// Representa un grupo clasificatorio dentro de la primera fase del torneo.
    /// </summary>
    public class Grupo
    {
        /// <summary>
        /// Identificador único del grupo.
        /// </summary>
        public int IdGrupo { get; set; }

        /// <summary>
        /// Letra o nombre que identifica el grupo.
        /// </summary>
        public string NombreGrupo { get; set; } = string.Empty;
    }
}
