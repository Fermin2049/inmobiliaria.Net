using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using System.Data;
using System.Data.SqlClient;


namespace asp.net.Models;

public class RepositorioPropietario
{
    readonly string ConnectionString = "Server=localhost;Database=inmobiliaria;User=root;Password=;";

    public IList<Propietario> GetPropietarios()
    {
        List<Propietario> propietarios = new List<Propietario>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"SELECT {nameof(Propietario.PropietarioID)}, 
                                {nameof(Propietario.Nombre)}, 
                                {nameof(Propietario.Apellido)}, 
                                {nameof(Propietario.Email)}, 
                                {nameof(Propietario.Telefono)}, 
                                {nameof(Propietario.Dni)} 
                         FROM propietarios";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        propietarios.Add(new Propietario
                        {
                            PropietarioID = reader.GetInt32(reader.GetOrdinal(nameof(Propietario.PropietarioID))),
                            Nombre = reader.GetString(reader.GetOrdinal(nameof(Propietario.Nombre))),
                            Apellido = reader.GetString(reader.GetOrdinal(nameof(Propietario.Apellido))),
                            Email = reader.GetString(reader.GetOrdinal(nameof(Propietario.Email))),
                            Telefono = reader.GetString(reader.GetOrdinal(nameof(Propietario.Telefono))),
                            Dni = reader.GetInt32(reader.GetOrdinal(nameof(Propietario.Dni)))
                        });
                    }
                }
            }
        }
        return propietarios;
    }

    public int CrearPropietario(Propietario propietario)
    {
        int id = 0;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"INSERT INTO propietarios 
                            ({nameof(Propietario.Nombre)}, 
                             {nameof(Propietario.Apellido)}, 
                             {nameof(Propietario.Email)}, 
                             {nameof(Propietario.Telefono)}, 
                             {nameof(Propietario.Dni)}) 
                         VALUES (@Nombre, @Apellido, @Email, @Telefono, @Dni); 
                         SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Nombre", propietario.Nombre);
                command.Parameters.AddWithValue("@Apellido", propietario.Apellido);
                command.Parameters.AddWithValue("@Email", propietario.Email);
                command.Parameters.AddWithValue("@Telefono", propietario.Telefono);
                command.Parameters.AddWithValue("@Dni", propietario.Dni);

             
                connection.Open();
                id = Convert.ToInt32(command.ExecuteScalar());
            }
        }
         
        return id;
    }

    public bool ActualizarPropietario(Propietario propietario)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"UPDATE propietarios 
                         SET {nameof(Propietario.Nombre)} = @Nombre, 
                             {nameof(Propietario.Apellido)} = @Apellido, 
                             {nameof(Propietario.Email)} = @Email, 
                             {nameof(Propietario.Telefono)} = @Telefono, 
                             {nameof(Propietario.Dni)} = @Dni 
                         WHERE {nameof(Propietario.PropietarioID)} = @PropietarioID;";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@PropietarioID", propietario.PropietarioID);
                command.Parameters.AddWithValue("@Nombre", propietario.Nombre);
                command.Parameters.AddWithValue("@Apellido", propietario.Apellido);
                command.Parameters.AddWithValue("@Email", propietario.Email);
                command.Parameters.AddWithValue("@Telefono", propietario.Telefono);
                command.Parameters.AddWithValue("@Dni", propietario.Dni);

                connection.Open();
                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }
    }

    public bool EliminarPropietario(int propietarioId)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"DELETE FROM propietarios 
                         WHERE {nameof(Propietario.PropietarioID)} = @PropietarioID;";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@PropietarioID", propietarioId);

                connection.Open();
                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }
    }


    

        public async Task<IList<Propietario>> BuscarEnVivo(string term)
{
    List<Propietario> resultados = new List<Propietario>();
    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
    {
        string sql = @"SELECT PropietarioID, Nombre, Apellido, Dni, Telefono, Email, Estado
                       FROM Propietarios
                       WHERE Nombre LIKE @term OR Apellido LIKE @term OR Dni LIKE @term";
        using (MySqlCommand command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@term", $"%{term}%");
            command.CommandType = CommandType.Text;
            await connection.OpenAsync();
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var propietario = new Propietario
                    {
                        PropietarioID = reader.GetInt32(reader.GetOrdinal(nameof(Propietario.PropietarioID))),
                            Nombre = reader.GetString(reader.GetOrdinal(nameof(Propietario.Nombre))),
                            Apellido = reader.GetString(reader.GetOrdinal(nameof(Propietario.Apellido))),
                            Dni = reader.GetInt32(reader.GetOrdinal(nameof(Propietario.Dni))),
                            Email = reader.GetString(reader.GetOrdinal(nameof(Propietario.Email))),
                            Telefono = reader.GetString(reader.GetOrdinal(nameof(Propietario.Telefono))),
                            Estado = reader.GetBoolean(reader.GetOrdinal(nameof(Propietario.Estado)))
                    };
                    resultados.Add(propietario);
                }
            }
        }
    }
    return resultados;
}



}
