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
        // Cadena de conexión con encoding UTF-8 explícito
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
                
                // Configurar el encoding UTF-8 después de abrir
                try
                {
                    using var cmd = connection.CreateCommand();
                    // Asegurar que Oracle use UTF-8 para todos los datos
                    cmd.CommandText = "ALTER SESSION SET NLS_CHARACTERSET='UTF8'";
                    cmd.ExecuteNonQuery();
                    
                    System.Diagnostics.Debug.WriteLine("Oracle NLS_CHARACTERSET set to UTF8");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Warning: Could not set NLS_CHARACTERSET: {ex.Message}");
                }
            }
            
            return connection;
        }
    }
}
