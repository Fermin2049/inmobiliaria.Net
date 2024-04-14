using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace asp.net.Models;

public class RepositorioUsuario
{
    readonly string ConnectionString = "Server=localhost;Database=inmobiliaria;User=root;Password=;";

    public IList<Usuario> GetUsuarios()
    {
        List<Usuario> usuarios = new List<Usuario>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"SELECT {nameof(Usuario.UsuarioID)}, 
                                {nameof(Usuario.Email)}, 
                                {nameof(Usuario.Contrasena)}, 
                                {nameof(Usuario.Rol)}, 
                                {nameof(Usuario.Avatar)} 
                         FROM usuarios";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuarios.Add(new Usuario
                        {
                            UsuarioID = reader.GetInt32(reader.GetOrdinal(nameof(Usuario.UsuarioID))),
                            Email = reader.GetString(reader.GetOrdinal(nameof(Usuario.Email))),
                            Contrasena = reader.GetString(reader.GetOrdinal(nameof(Usuario.Contrasena))),
                            Rol = reader.GetString(reader.GetOrdinal(nameof(Usuario.Rol))),
                            Avatar = reader.IsDBNull(reader.GetOrdinal(nameof(Usuario.Avatar))) ? null : (byte[])reader[nameof(Usuario.Avatar)]
                        });
                    }
                }
            }
        }
        return usuarios;
    }

    public int CrearUsuario(Usuario usuario)
    {
        int id = 0;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"INSERT INTO usuarios 
                            ({nameof(Usuario.Email)}, 
                             {nameof(Usuario.Contrasena)}, 
                             {nameof(Usuario.Rol)}, 
                             {nameof(Usuario.Avatar)}) 
                         VALUES (@Email, @Contrasena, @Rol, @Avatar); 
                         SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
                command.Parameters.AddWithValue("@Rol", usuario.Rol);
                command.Parameters.AddWithValue("@Avatar", (object)usuario.Avatar ?? DBNull.Value);

                connection.Open();
                id = Convert.ToInt32(command.ExecuteScalar());
            }
        }
        return id;
    }

    public bool ActualizarUsuario(Usuario usuario)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"UPDATE usuarios 
                         SET {nameof(Usuario.Email)} = @Email, 
                             {nameof(Usuario.Contrasena)} = @Contrasena, 
                             {nameof(Usuario.Rol)} = @Rol, 
                             {nameof(Usuario.Avatar)} = @Avatar 
                         WHERE {nameof(Usuario.UsuarioID)} = @UsuarioID;";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@UsuarioID", usuario.UsuarioID);
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
                command.Parameters.AddWithValue("@Rol", usuario.Rol);
                command.Parameters.AddWithValue("@Avatar", (object)usuario.Avatar ?? DBNull.Value);

                connection.Open();
                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }
    }

    public bool EliminarUsuario(int usuarioId)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"DELETE FROM usuarios 
                         WHERE {nameof(Usuario.UsuarioID)} = @UsuarioID;";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@UsuarioID", usuarioId);

                connection.Open();
                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }
   }
}
