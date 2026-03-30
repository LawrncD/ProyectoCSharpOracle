
using MiProyectoCSharp.Models;

namespace MiProyectoCSharp.Helpers
{
    /// <summary>
    /// Administra el estado global de la sesión del usuario en la aplicación (Singleton).
    /// </summary>
    public class SessionManager
    {
        private static SessionManager? _instance;

        /// <summary>
        /// Obtiene la instancia única activa del gestor de sesiones.
        /// </summary>
        public static SessionManager Instance => _instance ??= new SessionManager();

        /// <summary>
        /// Obtiene el usuario que se encuentra actualmente autenticado en el sistema.
        /// </summary>
        public Usuario? UsuarioActivo { get; private set; }

        /// <summary>
        /// Obtiene el identificador del registro de la bitácora vinculado a la sesión en curso.
        /// </summary>
        public int? BitacoraActivaId { get; private set; }

        private SessionManager() { }

        /// <summary>
        /// Inicializa los datos de la sesión para el usuario y bitácora especificados.
        /// </summary>
        /// <param name="usuario">La instancia del usuario que ha iniciado sesión.</param>
        /// <param name="idBitacora">El identificador de auditoría correspondiente al ingreso.</param>
        public void IniciarSesion(Usuario usuario, int idBitacora)
        {
            UsuarioActivo = usuario;
            BitacoraActivaId = idBitacora;
        }

        /// <summary>
        /// Finaliza la sesión limpiando la referencia del usuario actual y su bitácora.
        /// </summary>
        public void CerrarSesion()
        {
            UsuarioActivo = null;
            BitacoraActivaId = null;
        }

        /// <summary>
        /// Indica rápidamente si existe alguna referencia de usuario conectada válidamente.
        /// </summary>
        public bool HaySesionActiva => UsuarioActivo != null;
    }
}
