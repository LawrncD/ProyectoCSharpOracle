using System;

namespace MiProyectoCSharp.Domain.Entities
{
    /// <summary>
    /// Representa un registro en la bitácora del sistema para auditoría de accesos.
    /// </summary>
    public class Bitacora
    {
        /// <summary>
        /// Identificador único del registro de la bitácora.
        /// </summary>
        public int IdRegistro { get; set; }

        /// <summary>
        /// Usuario que realizó la acción.
        /// </summary>
        public Usuario Usuario { get; set; }

        /// <summary>
        /// Fecha y hora exacta en que el usuario inició la sesión.
        /// </summary>
        public DateTime FechaHoraIngreso { get; set; }

        /// <summary>
        /// Fecha y hora en que el usuario cerró la sesión.
        /// </summary>
        public DateTime? FechaHoraSalida { get; set; }
    }
}
