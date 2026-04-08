using System;
using System.Data;
using MiProyectoCSharp.Domain.Entities;
using ProyectoCSharpOracle.Domain.DomainServices;
using MiProyectoCSharp.Data;

namespace ProyectoCSharpOracle.Application.UseCases.Jugador
{
    /// <summary>
    /// Caso de uso para el método obtenerCantidadesJugadoresMenorDe21PorEquipo del servicio JugadorService.
    /// Solo usuarios esporádicos pueden consultar jugadores menores de 21 años por equipo.
    /// </summary>
    public class ObtenerCantidadesJugadoresMenorDe21PorEquipoUseCase
    {
        private readonly JugadorService _jugadorService;
        private readonly JugadorDAO _jugadorDAO;

        public ObtenerCantidadesJugadoresMenorDe21PorEquipoUseCase(JugadorService jugadorService, JugadorDAO jugadorDAO)
        {
            _jugadorService = jugadorService ?? throw new ArgumentNullException(nameof(jugadorService));
            _jugadorDAO = jugadorDAO ?? throw new ArgumentNullException(nameof(jugadorDAO));
        }

        /// <summary>
        /// Ejecuta el caso de uso para obtener la cantidad de jugadores menores de 21 años por equipo.
        /// </summary>
        /// <param name="usuario">El usuario que solicita la operación (debe ser esporádico).</param>
        /// <param name="idConfederacion">ID de la confederación (actualmente no usado pero disponible para filtrado).</param>
        /// <returns>DataTable con los resultados (equipo, menores_de_21).</returns>
        public DataTable Execute(Usuario usuario, int idConfederacion = 0)
        {
            // 1. Validación de dominio
            _jugadorService.obtenerCantidadesJugadoresMenorDe21PorEquipo(usuario, idConfederacion);

            // 2. Consulta
            return _jugadorDAO.ObtenerCantidadJugadoresMenoresDe21PorEquipo();
        }
    }
}
