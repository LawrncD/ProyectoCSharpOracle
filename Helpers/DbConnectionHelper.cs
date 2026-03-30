using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace MiProyectoCSharp.Helpers
{
    /// <summary>
    /// Proporciona funcionalidades auxiliares para gestionar la conexión a la base de datos Oracle.
    /// </summary>
    public class DbConnectionHelper
    {
        // NOTA: Ajusta la cadena de conexión según tu entorno Oracle (Host, Puerto, Servicio, Usuario, Password)
        // Ejemplo genérico para localhost XE
        private static readonly string ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=127.0.0.1)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XE)));User Id=copamundial;Password=copamundial;";

        /// <summary>
        /// Obtiene y abre una nueva conexión con la base de datos.
        /// </summary>
        /// <returns>Una instancia abierta de <see cref="OracleConnection"/>.</returns>
        public static OracleConnection GetConnection()
        {
            var connection = new OracleConnection(ConnectionString);
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            return connection;
        }
    }
}
