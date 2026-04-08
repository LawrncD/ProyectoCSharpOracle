/*using System;
using MiProyectoCSharp.Domain.Entities;
using ProyectoCSharpOracle.Domain.DomainServices;
using MiProyectoCSharp.Repository;

namespace ProyectoCSharpOracle.Application.UseCases.ConfederacionUC
{
    /// <summary>
    /// Caso de uso para el método obtenerValorEquiposPorConfederacion del servicio ConfederacionService.
    /// Solo usuarios no-esporádicos pueden obtener el valor de equipos de una confederación.
    /// </summary>
    public class ObtenerValorEquipoConfederacionUseCase
    {
        private readonly ConfederacionService _confederacionService;
        private readonly ConfederacionDAO _confederacionDAO;

        public ObtenerValorEquipoConfederacionUseCase(ConfederacionService confederacionService, ConfederacionDAO confederacionDAO)
        {
            _confederacionService = confederacionService ?? throw new ArgumentNullException(nameof(confederacionService));
            _confederacionDAO = confederacionDAO ?? throw new ArgumentNullException(nameof(confederacionDAO));
        }

        /// <summary>
        /// Ejecuta el caso de uso para obtener el valor de equipos de una confederación específica.
        /// </summary>
        /// <param name="usuario">El usuario que solicita la operación.</param>
        /// <param name="confederacion">La confederación a consultar.</param>
        /// <returns>La confederación con datos de valor de equipos.</returns>
        public List<Confederacion> Execute(Usuario usuario, Confederacion confederacion)
        {
            // 1. Validación de dominio
            var confValidada = _confederacionService.obtenerValorEquiposPorConfederacion(usuario, confederacion);

            // 2. La lógica específica se maneja en el DAO si es necesario
            return _confederacionDAO.ObtenerValorEquiposPorConfederacion(confValidada);
        }
    }
}
*/