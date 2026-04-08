using System;
using System.Data;
using MiProyectoCSharp.Domain.Entities;
using ProyectoCSharpOracle.Domain.DomainServices;
using MiProyectoCSharp.Data;

namespace ProyectoCSharpOracle.Application.UseCases.Jugador
{
    /// <summary>
    /// Caso de uso para el método obtenerJugadorMasCostosoPorCOnfederacion del servicio JugadorService.
    /// Solo usuarios esporádicos pueden consultar el jugador más costoso por confederación.
    /// </summary>
    public class ObtenerJugadorMasCostosoPorConfederacionUseCase
    {
        private readonly JugadorService _jugadorService;
        private readonly JugadorDAO _jugadorDAO;

        public ObtenerJugadorMasCostosoPorConfederacionUseCase(JugadorService jugadorService, JugadorDAO jugadorDAO)
        {
            _jugadorService = jugadorService ?? throw new ArgumentNullException(nameof(jugadorService));
            _jugadorDAO = jugadorDAO ?? throw new ArgumentNullException(nameof(jugadorDAO));
        }

        /// <summary>
        /// Ejecuta el caso de uso para obtener el jugador más costoso por confederación.
        /// </summary>
        /// <param name="usuario">El usuario que solicita la operación (debe ser esporádico).</param>
        /// <param name="idEquipo">ID del equipo (actualmente no usado en el parámetro pero se puede extender).</param>
        /// <returns>DataTable con los resultados (confederación, jugador, valor_mercado).</returns>
        public DataTable Execute(Usuario usuario, int idEquipo = 0)
        {
            // 1. Validación de dominio
            _jugadorService.obtenerJugadorMasCostosoPorCOnfederacion(usuario, idEquipo);

            // 2. Consulta
            return _jugadorDAO.ObtenerJugadorMasCostosoPorConfederacion();
        }
    }
}
