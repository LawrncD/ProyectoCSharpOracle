
using MiProyectoCSharp.Domain.Entities;
using MiProyectoCSharp.Repository;
using ProyectoCSharpOracle.Domain.DomainServices;

namespace ProyectoCSharpOracle.Application.UseCases.obtenerTodasConfederaciones

{
    /// <summary>
    /// Caso de uso para el método obtenerTodasConfederaciones del servicio de dominio ConfederacionService.
    /// Solo usuarios no-esporádicos pueden listar confederaciones.
    /// </summary>
    public class ObtenerTodasConfederacionesUseCase
    {
        private readonly ConfederacionService _confederacionService;
        private readonly ConfederacionDA _confederacionDAO;

        public ObtenerTodasConfederacionesUseCase(ConfederacionService confederacionService, ConfederacionDA confederacionDAO)
        {
            _confederacionService = confederacionService ?? throw new ArgumentNullException(nameof(confederacionService));
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
            _confederacionService.obtenerTodasConfederaciones(usuario);

            // 2. Consulta
            return _confederacionDAO.ObtenerTodas();
        }
    }
}
