using System;
using System.Data;
using MiProyectoCSharp.Domain.Entities;
using ProyectoCSharpOracle.Domain.DomainServices;
using MiProyectoCSharp.Repository;

namespace ProyectoCSharpOracle.Application.UseCases.JugadorUC
{
    /// <summary>
    /// Caso de uso para el método ObtenerJugadoresFiltrados del servicio JugadorService.
    /// Solo usuarios no-esporádicos pueden filtrar jugadores por peso, estatura y equipo.
    /// </summary>
    public class ObtenerJugadoresFiltradosUseCase
    {
        private readonly JugadorDAO _jugadorDAO;

        public ObtenerJugadoresFiltradosUseCase(JugadorDAO jugadorDAO)
        {
            _jugadorDAO = jugadorDAO ?? throw new ArgumentNullException(nameof(jugadorDAO));
        }

        /// <summary>
        /// Ejecuta el caso de uso para obtener jugadores filtrados por peso, estatura y equipo.
        /// </summary>
        /// <param name="usuario">El usuario que solicita la operación.</param>
        /// <param name="pesoMin">Peso mínimo.</param>
        /// <param name="pesoMax">Peso máximo.</param>
        /// <param name="estMin">Estatura mínima.</param>
        /// <param name="estMax">Estatura máxima.</param>
        /// <param name="posicion">Posición del jugador (filtrado adicional).</param>
        /// <param name="idEquipo">ID del equipo opcional (puede ser nulo).</param>
        /// <returns>DataTable con los resultados filtrados.</returns>
        public DataTable Execute(Usuario usuario, decimal pesoMin, decimal pesoMax, 
                                decimal estMin, decimal estMax, string posicion, int? idEquipo = null)
        {
            // 1. Validación de parámetros
            if (pesoMin < 0 || pesoMax < 0 || estMin < 0 || estMax < 0)
                throw new ArgumentException("Los valores de peso y estatura no pueden ser negativos.");

            if (pesoMin >= pesoMax || estMin >= estMax)
                throw new ArgumentException("El valor mínimo no puede ser mayor o igual al máximo.");

            // 2. Validación de dominio
            AuthorizationService.ValidatePermission(usuario, Operation.ManagePlayers);

            // 3. Consulta con filtros
            return _jugadorDAO.ObtenerJugadoresFiltrados(pesoMin, pesoMax, estMin, estMax, idEquipo);
        }
    }
}
