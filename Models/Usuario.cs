using System;
using MiProyectoCSharp.Enums;

namespace MiProyectoCSharp.Models
{
    /// <summary>
    /// Representa a un usuario del sistema que puede acceder con credenciales.
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        public int IdUsuario { get; set; }

        /// <summary>
        /// Nombre de inicio de sesión o alias del usuario.
        /// </summary>
        public string NombreUsuario { get; set; } = string.Empty;

        /// <summary>
        /// Hash de la contraseña utilizada por el usuario para ingresar al sistema.
        /// </summary>
        public string ContrasenaHash { get; set; } = string.Empty;

        /// <summary>
        /// Nivel de permisos o rol que tiene el usuario dentro de la plataforma.
        /// </summary>
        public TipoUsuario Tipo { get; set; }

        /// <summary>
        /// Fecha exacta de cuándo fue creada la cuenta de usuario.
        /// </summary>
        public DateTime FechaCreacion { get; set; }
    }
}
