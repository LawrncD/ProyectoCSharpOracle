using System;
using MiProyectoCSharp.Domain.Entities;
using ProyectoCSharpOracle.Domain.DomainServices;
using MiProyectoCSharp.Data;

namespace ProyectoCSharpOracle.Application.UseCases.Equipo
{
    /// <summary>
    /// Caso de uso para el método obtenerValorEquiposPorConfederacion del servicio EquipoService.
    /// Solo usuarios no-esporádicos pueden consultar valores de equipos por confederación.
    /// </summary>
    public class ObtenerValorEquiposPorConfederacionUseCase
    {
        private readonly EquipoService _equipoService;
        private readonly EquipoDAO _equipoDAO;

        public ObtenerValorEquiposPorConfederacionUseCase(EquipoService equipoService, EquipoDAO equipoDAO)
        {
            _equipoService = equipoService ?? throw new ArgumentNullException(nameof(equipoService));
            _equipoDAO = equipoDAO ?? throw new ArgumentNullException(nameof(equipoDAO));
        }

        /// <summary>
        /// Ejecuta el caso de uso para obtener valores de equipos por confederación.
        /// </summary>
        /// <param name="usuario">El usuario que solicita la operación.</param>
        /// <param name="idConfederacion">El ID de la confederación a consultar.</param>
        public void Execute(Usuario usuario, int idConfederacion)
        {
            // 1. Validación adicional
            if (idConfederacion <= 0)
                throw new ArgumentException("El ID de confederación debe ser mayor a 0.", nameof(idConfederacion));

            // 2. Validación de dominio
            _equipoService.obtenerValorEquiposPorConfederacion(usuario, idConfederacion);

            // 3. Consulta
            // Nota: El DAO debería tener un método para esto, o se puede usar ObtenerTodos filtrado
        }
    }
}
