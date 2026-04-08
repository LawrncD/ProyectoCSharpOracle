using System;
using MiProyectoCSharp.Domain.Entities;
using ProyectoCSharpOracle.Domain.DomainServices;
using MiProyectoCSharp.Repository;

namespace ProyectoCSharpOracle.Application.UseCases.RegistroUsuariosUC
{
    /// <summary>
    /// Caso de uso para el método registrarSalida del servicio de dominio.
    /// Los usuarios esporádicos no pueden registrar salidas.
    /// </summary>
    public class RegistrarSalidaUseCase
    {
        private readonly BitacoraDAO _bitacoraDAO;

        public RegistrarSalidaUseCase(BitacoraDAO bitacoraDAO)
        {
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
            AuthorizationService.ValidatePermission(usuario, Operation.RegisterExit);

            // 2. Persistencia
            _bitacoraDAO.RegistrarSalida(idBitacora);
        }
    }
}
