using System;
using System.Data;
using MiProyectoCSharp.Domain.Entities;
using ProyectoCSharpOracle.Domain.DomainServices;
using MiProyectoCSharp.Repository;

namespace ProyectoCSharpOracle.Application.UseCases.EquipoUC
{
    /// <summary>
    /// Caso de uso para el método obtenerEquiposMasCarosPorCiudad del servicio EquipoService.
    /// Solo usuarios esporádicos pueden consultar el equipo más caro por país anfitrión.
    /// </summary>
    public class ObtenerEquiposMasCarosPorCiudadUseCase
    {
        private readonly EquipoDAO _equipoDAO;

        public ObtenerEquiposMasCarosPorCiudadUseCase(EquipoDAO equipoDAO)
        {
            _equipoDAO = equipoDAO ?? throw new ArgumentNullException(nameof(equipoDAO));
        }

        /// <summary>
        /// Ejecuta el caso de uso para obtener los equipos más caros por país anfitrión.
        /// </summary>
        /// <param name="usuario">El usuario que solicita la operación (debe ser esporádico).</param>
        /// <returns>DataTable con los resultados.</returns>
        public DataTable Execute(Usuario usuario)
        {
            // 1. Validación de dominio
            AuthorizationService.ValidatePermission(usuario, Operation.QueryMostExpensiveTeamByCountry);

            // 2. Consulta
            return _equipoDAO.ObtenerEquipoMasCostosoPorPais();
        }
    }
}
