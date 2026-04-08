using System;
using MiProyectoCSharp.Domain.Entities;
using ProyectoCSharpOracle.Domain.DomainServices;
using MiProyectoCSharp.Repository;

namespace ProyectoCSharpOracle.Application.UseCases.JugadorUC
{
    /// <summary>
    /// Caso de uso para el método insertarJugador del servicio de dominio JugadorService.
    /// Solo usuarios no-esporádicos pueden insertar jugadores.
    /// </summary>
    public class InsertarJugadorUseCase
    {
        private readonly JugadorDAO _jugadorDAO;

        public InsertarJugadorUseCase(JugadorDAO jugadorDAO)
        {
            _jugadorDAO = jugadorDAO ?? throw new ArgumentNullException(nameof(jugadorDAO));
        }

        /// <summary>
        /// Ejecuta el caso de uso para insertar un nuevo jugador.
        /// </summary>
        /// <param name="usuario">El usuario que solicita la operación.</param>
        /// <param name="jugador">El jugador completo a insertar (con equipo).</param>
        /// <returns>True si se insertó exitosamente, false en caso contrario.</returns>
        public bool Execute(Usuario usuario, Jugador jugador)
        {
            // 1. Validación de dominio
            AuthorizationService.ValidatePermission(usuario, Operation.ManagePlayers);

            // 2. Persistencia
            return _jugadorDAO.Insertar(jugador);
        }
    }
}
