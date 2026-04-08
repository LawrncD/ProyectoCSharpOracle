using System;

namespace MiProyectoCSharp.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando un usuario no tiene permisos suficientes para una operación.
    /// </summary>
    public class InsufficientPermissionsException : Exception
    {
        public InsufficientPermissionsException(string message) : base(message) { }
        public InsufficientPermissionsException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Excepción lanzada cuando una operación no está permitida para el rol del usuario.
    /// </summary>
    public class OperationNotAllowedForRoleException : Exception
    {
        public OperationNotAllowedForRoleException(string message) : base(message) { }
        public OperationNotAllowedForRoleException(string message, Exception innerException) : base(message, innerException) { }
    }
}