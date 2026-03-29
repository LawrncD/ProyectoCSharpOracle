using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace MiProyectoCSharp.Helpers
{
    public class DbConnectionHelper
    {
        // NOTA: Ajusta la cadena de conexión según tu entorno Oracle (Host, Puerto, Servicio, Usuario, Password)
        // Ejemplo genérico para localhost XE
        private static readonly string ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XE)));User Id=copamundial;Password=copamundial;";

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
