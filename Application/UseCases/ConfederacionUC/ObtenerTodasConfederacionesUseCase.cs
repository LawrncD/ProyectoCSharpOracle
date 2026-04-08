
using MiProyectoCSharp.Domain.Entities;
using MiProyectoCSharp.Repository;
using ProyectoCSharpOracle.Domain.DomainServices;

namespace ProyectoCSharpOracle.Application.UseCases.ConfederacionUC

{
    /// <summary>
    /// Caso de uso para el método obtenerTodasConfederaciones del servicio de dominio ConfederacionService.
    /// Solo usuarios no-esporádicos pueden listar confederaciones.
    /// </summary>
    public class ObtenerTodasConfederacionesUseCase
    {
        private readonly ConfederacionDAO _confederacionDAO;

        public ObtenerTodasConfederacionesUseCase(ConfederacionDAO confederacionDAO)
        {
            _confederacionDAO = confederacionDAO ?? throw new ArgumentNullException(nameof(confederacionDAO));
        }

        /// <summary>
        /// Ejecuta el caso de uso para obtener todas las confederaciones.
        /// </summary>
        /// <param name="usuario">El usuario que solicita la operación.</param>
        /// <returns>Lista de todas las confederaciones disponibles.</returns>
        public List<Confederacion> Execute(Usuario usuario)
        {
            // 1. Validación de dominio
            AuthorizationService.ValidatePermission(usuario, Operation.ManageConfederations);
            // 2. Consulta
            return _confederacionDAO.ObtenerTodas();
        }
    }
}
