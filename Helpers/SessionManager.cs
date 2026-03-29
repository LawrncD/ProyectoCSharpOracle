using MiProyectoCSharp.Models;

namespace MiProyectoCSharp.Helpers
{
    public class SessionManager
    {
        private static SessionManager? _instance;
        public static SessionManager Instance => _instance ??= new SessionManager();

        public Usuario? UsuarioActivo { get; private set; }
        public int? BitacoraActivaId { get; private set; }

        private SessionManager() { }

        public void IniciarSesion(Usuario usuario, int idBitacora)
        {
            UsuarioActivo = usuario;
            BitacoraActivaId = idBitacora;
        }

        public void CerrarSesion()
        {
            UsuarioActivo = null;
            BitacoraActivaId = null;
        }

        public bool HaySesionActiva => UsuarioActivo != null;
    }
}
