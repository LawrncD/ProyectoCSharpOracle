using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiProyectoCSharp.Domain.Entities;
using MiProyectoCSharp.Enums;

namespace ProyectoCSharpOracle.Domain.DomainServices
{
    public class EquipoService
    {
        public void obtenerTodas(Usuario usuario)
        {
            AuthorizationService.ValidatePermission(usuario, Operation.ManageTeams);
        }

        public Equipo insertarEquipo(Usuario usuario, Equipo equipo)
        {
            AuthorizationService.ValidatePermission(usuario, Operation.ManageTeams);
            return equipo;
        }

        public void obtenerValorEquiposPorConfederacion(Usuario usuario, int idConfederacion)
        {
            AuthorizationService.ValidatePermission(usuario, Operation.ManageTeams);
        }
        public void obtenerEquiposMasCarosPorCiudad(Usuario usuario)
        {
            AuthorizationService.ValidatePermission(usuario, Operation.QueryMostExpensiveTeamByCountry);
        }   
        
    }
}