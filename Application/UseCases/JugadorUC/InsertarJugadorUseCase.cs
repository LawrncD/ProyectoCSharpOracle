using System;
using MiProyectoCSharp.Domain.Entities;
using ProyectoCSharpOracle.Domain.DomainServices;
using MiProyectoCSharp.Data;

namespace ProyectoCSharpOracle.Application.UseCases.Jugador
{
    /// <summary>
    /// Caso de uso para el método insertarJugador del servicio de dominio JugadorService.
    /// Solo usuarios no-esporádicos pueden insertar jugadores.
    /// </summary>
    public class InsertarJugadorUseCase
    {
        private readonly JugadorService _jugadorService;
        private readonly JugadorDAO _jugadorDAO;

        public InsertarJugadorUseCase(JugadorService jugadorService, JugadorDAO jugadorDAO)
        {
            _jugadorService = jugadorService ?? throw new ArgumentNullException(nameof(jugadorService));
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
            var jugadorValidado = _jugadorService.insertarJugador(usuario, jugador);

            // 2. Persistencia
            return _jugadorDAO.Insertar(jugadorValidado);
        }
    }
}
