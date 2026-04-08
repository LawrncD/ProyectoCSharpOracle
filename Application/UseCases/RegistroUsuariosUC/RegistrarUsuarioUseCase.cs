using System;
using MiProyectoCSharp.Domain.Entities;
using ProyectoCSharpOracle.Domain.DomainServices;
using MiProyectoCSharp.Data;

namespace ProyectoCSharpOracle.Application.UseCases.RegistroUsuarios
{
    /// <summary>
    /// Caso de uso para el método registrarUsuario del servicio de dominio.
    /// Solo administradores pueden registrar usuarios.
    /// </summary>
    public class RegistrarUsuarioUseCase
    {
        private readonly RegistroUsuariosService _registroService;
        private readonly UsuarioDAO _usuarioDAO;

        public RegistrarUsuarioUseCase(RegistroUsuariosService registroService, UsuarioDAO usuarioDAO)
        {
            _registroService = registroService ?? throw new ArgumentNullException(nameof(registroService));
            _usuarioDAO = usuarioDAO ?? throw new ArgumentNullException(nameof(usuarioDAO));
        }

        /// <summary>
        /// Ejecuta el caso de uso para registrar un nuevo usuario.
        /// </summary>
        /// <param name="usuarioRegistrador">El administrador que realiza el registro.</param>
        /// <param name="usuarioNuevo">El nuevo usuario a registrar.</param>
        /// <returns>El usuario registrado con ID asignado.</returns>
        public bool Execute(Usuario usuarioRegistrador, Usuario usuarioNuevo)
        {
            // 1. Validación de dominio
            _registroService.registrarUsuario(usuarioRegistrador, usuarioNuevo);

            // 2. Persistencia
            return _usuarioDAO.Insertar(usuarioNuevo);
        }
    }
}
