using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiProyectoCSharp.Domain.Entities;

namespace ProyectoCSharpOracle.Domain.DomainServices
{
    public class JugadorService
    {
        public void obtenerTodosJugadores(Usuario usuario)
        {
            AuthorizationService.ValidatePermission(usuario, Operation.ManagePlayers);
        }
        public Jugador insertarJugador(Usuario usuario, Jugador jugador)
        {
            AuthorizationService.ValidatePermission(usuario, Operation.ManagePlayers);
            return jugador; 
        }
        public void obtenerJugadorMasCostosoPorCOnfederacion(Usuario usuario, int idEquipo)
        {
            AuthorizationService.ValidatePermission(usuario, Operation.QueryExpensivePlayerByConfederation);
        }
        public void obtenerCantidadesJugadoresMenorDe21PorEquipo(Usuario usuario, int idConfederacion)
        {
            AuthorizationService.ValidatePermission (usuario, Operation.QueryYoungPlayersByTeam);     
        }
        public void ObtenerJugadoresFiltrados(Usuario usuario, string posicion)
        {
            AuthorizationService.ValidatePermission(usuario, Operation.ManagePlayers);  
        }
    }
}