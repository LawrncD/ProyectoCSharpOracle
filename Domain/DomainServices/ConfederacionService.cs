using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiProyectoCSharp.Domain.Entities;
using MiProyectoCSharp.Enums;

namespace ProyectoCSharpOracle.Domain.DomainServices
{
    public class ConfederacionService
    {
        public void obtenerTodasConfederaciones(Usuario usuario)
        {
            AuthorizationService.ValidatePermission(usuario, Operation.ManageConfederations);
        }

        public Confederacion obtenerValorEquiposPorConfederacion(Usuario usuario, Confederacion confederacion)
        {
            AuthorizationService.ValidatePermission(usuario, Operation.ManageConfederations);
            return confederacion;
        }
    }
}

