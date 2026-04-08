using System;
using System.Collections.Generic;
using MiProyectoCSharp.Domain.Entities;
using ProyectoCSharpOracle.Domain.DomainServices;
using MiProyectoCSharp.Data;

namespace ProyectoCSharpOracle.Application.UseCases.Equipo
{
    /// <summary>
    /// Caso de uso para el método obtenerTodas del servicio de dominio EquipoService.
    /// Solo usuarios no-esporádicos pueden listar todos los equipos.
    /// </summary>
    public class ObtenerTodosEquiposUseCase
    {
        private readonly EquipoService _equipoService;
        private readonly EquipoDAO _equipoDAO;

        public ObtenerTodosEquiposUseCase(EquipoService equipoService, EquipoDAO equipoDAO)
        {
            _equipoService = equipoService ?? throw new ArgumentNullException(nameof(equipoService));
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
            _equipoService.obtenerTodas(usuario);

            // 2. Consulta
            return _equipoDAO.ObtenerTodos();
        }
    }
}
