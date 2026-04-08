using System;
using MiProyectoCSharp.Domain.Entities;
using MiProyectoCSharp.Enums;
using MiProyectoCSharp.Exceptions;

namespace ProyectoCSharpOracle.Domain.DomainServices
{
    /// <summary>
    /// Servicio de dominio para centralizar validaciones de autorización basadas en roles de usuario.
    /// Evita repetición de lógica en otros servicios de dominio.
    /// </summary>
    public static class AuthorizationService
    {
        public static void ValidatePermission(Usuario usuario, Operation operation)
        {
            if (!HasPermission(usuario, operation))
            {
                // Lanzar excepción específica dependiendo del rol del usuario
                switch (usuario.Tipo)
                {
                    case TipoUsuario.Administrador:
                        throw new InsufficientPermissionsException($"El administrador {usuario.NombreUsuario} no tiene permisos para la operación '{operation}'. Solo puede gestionar usuarios.");
                    
                    case TipoUsuario.Tradicional:
                        throw new OperationNotAllowedForRoleException($"La operación '{operation}' no está permitida para usuarios tradicionales.");
                    
                    case TipoUsuario.Esporadico:
                        throw new OperationNotAllowedForRoleException($"El usuario esporádico {usuario.NombreUsuario} solo puede realizar consultas específicas. Operación '{operation}' no permitida.");
                    
                    default:
                        throw new InsufficientPermissionsException($"Rol desconocido para el usuario {usuario.NombreUsuario}.");
                }
            }
        }

        /// <summary>
        /// Verifica si el usuario tiene permiso para la operación (sin lanzar excepción).
        /// </summary>
        /// <param name="usuario">El usuario a validar.</param>
        /// <param name="operation">La operación a verificar.</param>
        /// <returns>True si tiene permiso, false en caso contrario.</returns>
        public static bool HasPermission(Usuario usuario, Operation operation)
        {
            switch (usuario.Tipo)
            {
                case TipoUsuario.Administrador:
                    // Admin solo puede agregar usuarios y gestionar usuarios
                    return operation == Operation.AddUser || operation == Operation.ManageUsers || operation == Operation.RegisterExit;

                case TipoUsuario.Tradicional:
                    // Tradicional puede hacer todo menos agregar usuarios
                    return operation != Operation.AddUser && operation != Operation.ManageUsers;

                case TipoUsuario.Esporadico:
                    // Esporadico solo consultas específicas
                    return operation == Operation.QueryExpensivePlayerByConfederation ||
                           operation == Operation.QueryMatchesInStadium ||
                           operation == Operation.QueryMostExpensiveTeamByCountry ||
                           operation == Operation.QueryYoungPlayersByTeam;

                default:
                    return false;
            }
        }

        internal static void ValidatePermission(Usuario usuario, object manageTeams)
        {
            throw new NotImplementedException();
        }
    }
}