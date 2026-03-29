using System;
using MiProyectoCSharp.Enums;

namespace MiProyectoCSharp.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string ContrasenaHash { get; set; } = string.Empty;
        public TipoUsuario Tipo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
