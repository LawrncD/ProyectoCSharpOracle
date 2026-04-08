

using System;
using Oracle.ManagedDataAccess.Client;
using MiProyectoCSharp.Helpers;
using MiProyectoCSharp.Domain.Entities;
using System.Data;

namespace MiProyectoCSharp.Repository
{
    public class BitacoraDAO
    {
        public int RegistrarIngreso(Usuario usuario)
        {
            int idUsuario = usuario.IdUsuario;
            using var connection = DbConnectionHelper.GetConnection();
            // Insertamos y devolvemos el id generado
            string sql = @"
                INSERT INTO Bitacora (id_usuario, fecha_hora_ingreso) 
                VALUES (:userId, CURRENT_TIMESTAMP) 
                RETURNING id_registro INTO :outId";

            using var cmd = new OracleCommand(sql, connection);
            cmd.Parameters.Add(new OracleParameter("userId", idUsuario));
            
            var outParam = new OracleParameter("outId", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outParam);

            cmd.ExecuteNonQuery();
            
            // Convertir el id retornado (puede ser OracleDecimal)
            return Convert.ToInt32(outParam.Value.ToString());
        }

        public void RegistrarSalida(int idRegistro)
        {
            using var connection = DbConnectionHelper.GetConnection();
            string sql = "UPDATE Bitacora SET fecha_hora_salida = CURRENT_TIMESTAMP WHERE id_registro = :id";
            using var cmd = new OracleCommand(sql, connection);
            cmd.Parameters.Add(new OracleParameter("id", idRegistro));
            
            cmd.ExecuteNonQuery();
        }
    }
}
