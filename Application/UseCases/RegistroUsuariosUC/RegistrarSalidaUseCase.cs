using System;
using MiProyectoCSharp.Domain.Entities;
using ProyectoCSharpOracle.Domain.DomainServices;
using MiProyectoCSharp.Data;

namespace ProyectoCSharpOracle.Application.UseCases.RegistroUsuarios
{
    /// <summary>
    /// Caso de uso para el método registrarSalida del servicio de dominio.
    /// Los usuarios esporádicos no pueden registrar salidas.
    /// </summary>
    public class RegistrarSalidaUseCase
    {
        private readonly RegistroUsuariosService _registroService;
        private readonly BitacoraDAO _bitacoraDAO;

        public RegistrarSalidaUseCase(RegistroUsuariosService registroService, BitacoraDAO bitacoraDAO)
        {
            _registroService = registroService ?? throw new ArgumentNullException(nameof(registroService));
            _bitacoraDAO = bitacoraDAO ?? throw new ArgumentNullException(nameof(bitacoraDAO));
        }

        /// <summary>
        /// Ejecuta el caso de uso para registrar la salida de un usuario.
        /// </summary>
        /// <param name="usuario">El usuario que está saliendo.</param>
        /// <param name="idBitacora">El ID del registro de bitácora a cerrar.</param>
        public void Execute(Usuario usuario, int idBitacora)
        {
            // 1. Validación de dominio
            _registroService.registrarSalida(usuario);

            // 2. Persistencia
            _bitacoraDAO.RegistrarSalida(idBitacora);
        }
    }
}
