using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using MiProyectoCSharp.Domain.Entities;
using MiProyectoCSharp.Helpers;
using MiProyectoCSharp.Enums;

namespace MiProyectoCSharp.Repository
{
    public class UsuarioDAO
    {
        public Usuario? ValidarCredenciales(string nombreUsuario, string contrasenaPlana)
        {
            using var connection = DbConnectionHelper.GetConnection();
            string sql = "SELECT id_usuario, nombre_usuario, contrasena_hash, tipo_usuario, fecha_creacion FROM Usuario WHERE nombre_usuario = :nombreUsuario";
            using var cmd = new OracleCommand(sql, connection);
            cmd.Parameters.Add(new OracleParameter("nombreUsuario", nombreUsuario));

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string hash = reader["contrasena_hash"].ToString() ?? "";
                if (BCrypt.Net.BCrypt.Verify(contrasenaPlana, hash))
                {
                    return new Usuario
                    {
                        IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                        NombreUsuario = reader["nombre_usuario"].ToString() ?? "",
                        ContrasenaHash = hash,
                        Tipo = Enum.Parse<TipoUsuario>(reader["tipo_usuario"].ToString() ?? "Tradicional"),
                        FechaCreacion = Convert.ToDateTime(reader["fecha_creacion"])
                    };
                }
            }
            return null;
        }

        public List<Usuario> ObtenerTodos()
        {
            var usuarios = new List<Usuario>();
            using var connection = DbConnectionHelper.GetConnection();
            string sql = "SELECT id_usuario, nombre_usuario, tipo_usuario, fecha_creacion FROM Usuario";
            using var cmd = new OracleCommand(sql, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                usuarios.Add(new Usuario
                {
                    IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                    NombreUsuario = reader["nombre_usuario"].ToString() ?? "",
                    Tipo = Enum.Parse<TipoUsuario>(reader["tipo_usuario"].ToString() ?? "Tradicional"),
                    FechaCreacion = Convert.ToDateTime(reader["fecha_creacion"])
                });
            }
            return usuarios;
        }

        public bool Insertar(Usuario usuario)
        {
            using var connection = DbConnectionHelper.GetConnection();
            string sql = "INSERT INTO Usuario (nombre_usuario, contrasena_hash, tipo_usuario) VALUES (:nombre, :hash, :tipo)";
            using var cmd = new OracleCommand(sql, connection);
            cmd.Parameters.Add(new OracleParameter("nombre", usuario.NombreUsuario));
            cmd.Parameters.Add(new OracleParameter("hash", BCrypt.Net.BCrypt.HashPassword(usuario.ContrasenaHash)));
            cmd.Parameters.Add(new OracleParameter("tipo", usuario.Tipo.ToString()));

            return cmd.ExecuteNonQuery() > 0;
        }
        
        public bool Eliminar(int idUsuario)
        {
            using var connection = DbConnectionHelper.GetConnection();
            string sql = "DELETE FROM Usuario WHERE id_usuario = :id";
            using var cmd = new OracleCommand(sql, connection);
            cmd.Parameters.Add(new OracleParameter("id", idUsuario));
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
