using System;
using MiProyectoCSharp.Domain.Entities;
using ProyectoCSharpOracle.Domain.DomainServices;
using MiProyectoCSharp.Repository;

namespace ProyectoCSharpOracle.Application.UseCases.EquipoUC
{
    /// <summary>
    /// Caso de uso para el método insertarEquipo del servicio de dominio EquipoService.
    /// Solo usuarios no-esporádicos pueden insertar equipos.
    /// </summary>
    public class InsertarEquipoUseCase
    {
        private readonly EquipoDAO _equipoDAO;

        public InsertarEquipoUseCase(EquipoDAO equipoDAO)
        {
            _equipoDAO = equipoDAO ?? throw new ArgumentNullException(nameof(equipoDAO));
        }

        /// <summary>
        /// Ejecuta el caso de uso para insertar un nuevo equipo.
        /// </summary>
        /// <param name="usuario">El usuario que solicita la operación.</param>
        /// <param name="equipo">El equipo completo a insertar (con confederación).</param>
        /// <returns>True si se insertó exitosamente, false en caso contrario.</returns>
        public bool Execute(Usuario usuario, Equipo equipo)
        {
            // 1. Validación de dominio
            AuthorizationService.ValidatePermission(usuario, Operation.ManageTeams);

            // 2. Persistencia
            return _equipoDAO.Insertar(equipo);
        }
    }
}
