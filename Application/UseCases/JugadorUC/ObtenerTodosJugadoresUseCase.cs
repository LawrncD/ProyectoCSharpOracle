using System;
using System.Collections.Generic;
using MiProyectoCSharp.Domain.Entities;
using ProyectoCSharpOracle.Domain.DomainServices;
using MiProyectoCSharp.Repository;
namespace ProyectoCSharpOracle.Application.UseCases.JugadorUC;

    /// <summary>
    /// Caso de uso para el método obtenerTodosJugadores del servicio de dominio JugadorService.
    /// Solo usuarios no-esporádicos pueden listar todos los jugadores.
    /// </summary>
    public class ObtenerTodosJugadoresUseCase
    {
        private readonly JugadorDAO _jugadorDAO;

        public ObtenerTodosJugadoresUseCase(JugadorDAO jugadorDAO)
        {
            _jugadorDAO = jugadorDAO ?? throw new ArgumentNullException(nameof(jugadorDAO));
        }

        /// <summary>
        /// Ejecuta el caso de uso para obtener todos los jugadores.
        /// </summary>
        /// <param name="usuario">El usuario que solicita la operación.</param>
        /// <returns>Lista de todos los jugadores disponibles.</returns>
        public List<Jugador> Execute(Usuario usuario)
        {
            // 1. Validación de dominio
            AuthorizationService.ValidatePermission(usuario, Operation.ManagePlayers);

            // 2. Consulta
            return _jugadorDAO.ObtenerTodos();
        }
    }

