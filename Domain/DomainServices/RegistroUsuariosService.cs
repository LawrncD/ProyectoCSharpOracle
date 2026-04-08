using System;
using System.Threading.Tasks;
using MiProyectoCSharp.Domain.Entities;
using MiProyectoCSharp.Enums;

namespace ProyectoCSharpOracle.Domain.DomainServices
{
    public class RegistroUsuariosService
    {
       public Usuario registrarUsuario(Usuario usuarioRegistrador, Usuario usuarioNuevo){
            AuthorizationService.ValidatePermission(usuarioRegistrador, Operation.AddUser);
            return usuarioNuevo;
        }
        public void registrarSalida(Usuario usuario)
        {
            AuthorizationService.ValidatePermission(usuario, Operation.RegisterExit);
        }
        public void listarEstadiosUsuario(Usuario usuario)
        {
            AuthorizationService.ValidatePermission(usuario, Operation.QueryMatchesInStadium);
        }
    }   
}