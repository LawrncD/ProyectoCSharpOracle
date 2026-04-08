using System;
using System.Collections.Generic;
using MiProyectoCSharp.Domain.Entities;
using ProyectoCSharpOracle.Domain.DomainServices;
using MiProyectoCSharp.Repository;

namespace ProyectoCSharpOracle.Application.UseCases.EquipoUC
{
    /// <summary>
    /// Caso de uso para el método obtenerTodas del servicio de dominio EquipoService.
    /// Solo usuarios no-esporádicos pueden listar todos los equipos.
    /// </summary>
    public class ObtenerTodosEquiposUseCase
    {
        private readonly EquipoDAO _equipoDAO;

        public ObtenerTodosEquiposUseCase(EquipoDAO equipoDAO)
        {
            _equipoDAO = equipoDAO ?? throw new ArgumentNullException(nameof(equipoDAO));
        }

        /// <summary>
        /// Ejecuta el caso de uso para obtener todos los equipos.
        /// </summary>
        /// <param name="usuario">El usuario que solicita la operación.</param>
        /// <returns>Lista de todos los equipos disponibles.</returns>
        public List<Equipo> Execute(Usuario usuario)
        {
            // 1. Validación de dominio
            AuthorizationService.ValidatePermission(usuario, Operation.ManageTeams);

            // 2. Consulta
            return _equipoDAO.ObtenerTodos();
        }
    }
}
