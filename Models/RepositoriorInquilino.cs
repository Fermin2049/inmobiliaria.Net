using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace asp.net.Models;

public class RepositorioInquilino
{
    readonly string ConnectionString = "Server=localhost;Database=inmobiliaria;User=root;Password=;";

    public IList<Inquilino> GetInquilinos()
    {
        List<Inquilino> inquilinos = new List<Inquilino>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"SELECT {nameof(Inquilino.InquilinoID)}, 
                                {nameof(Inquilino.Nombre)}, 
                                {nameof(Inquilino.Apellido)}, 
                                {nameof(Inquilino.Dni)}, 
                                {nameof(Inquilino.Telefono)}, 
                                {nameof(Inquilino.Email)}, 
                                {nameof(Inquilino.Estado)} 
                         FROM inquilinos";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        inquilinos.Add(new Inquilino
                        {
                            InquilinoID = reader.GetInt32(reader.GetOrdinal(nameof(Inquilino.InquilinoID))),
                            Nombre = reader.GetString(reader.GetOrdinal(nameof(Inquilino.Nombre))),
                            Apellido = reader.GetString(reader.GetOrdinal(nameof(Inquilino.Apellido))),
                            Dni = reader.GetInt32(reader.GetOrdinal(nameof(Inquilino.Dni))), // Este es correcto
                            Telefono = reader.GetString(reader.GetOrdinal(nameof(Inquilino.Telefono))),
                            Email = reader.GetString(reader.GetOrdinal(nameof(Inquilino.Email))),
                            Estado = reader.GetBoolean(reader.GetOrdinal(nameof(Inquilino.Estado)))
                        });
                    }
                }
            }
        }
        return inquilinos;
    }

    public int CrearInquilino(Inquilino inquilino)
    {
        int id = 0;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"INSERT INTO inquilinos 
                            ({nameof(Inquilino.Nombre)}, 
                             {nameof(Inquilino.Apellido)}, 
                             {nameof(Inquilino.Dni)}, 
                             {nameof(Inquilino.Telefono)}, 
                             {nameof(Inquilino.Email)}, 
                             {nameof(Inquilino.Estado)}) 
                         VALUES (@Nombre, @Apellido, @Dni, @Telefono, @Email, @Estado); 
                         SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Nombre", inquilino.Nombre);
                command.Parameters.AddWithValue("@Apellido", inquilino.Apellido);
                command.Parameters.AddWithValue("@Dni", inquilino.Dni);
                command.Parameters.AddWithValue("@Telefono", inquilino.Telefono);
                command.Parameters.AddWithValue("@Email", inquilino.Email);
                command.Parameters.AddWithValue("@Estado", inquilino.Estado);

                connection.Open();
                id = Convert.ToInt32(command.ExecuteScalar());
            }
        }
        return id;
    }

    public bool ActualizarInquilino(Inquilino inquilino)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"UPDATE inquilinos 
                         SET {nameof(Inquilino.Nombre)} = @Nombre, 
                             {nameof(Inquilino.Apellido)} = @Apellido, 
                             {nameof(Inquilino.Dni)} = @Dni, 
                             {nameof(Inquilino.Telefono)} = @Telefono, 
                             {nameof(Inquilino.Email)} = @Email, 
                             {nameof(Inquilino.Estado)} = @Estado 
                         WHERE {nameof(Inquilino.InquilinoID)} = @InquilinoID;";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@InquilinoID", inquilino.InquilinoID);
                command.Parameters.AddWithValue("@Nombre", inquilino.Nombre);
                command.Parameters.AddWithValue("@Apellido", inquilino.Apellido);
                command.Parameters.AddWithValue("@Dni", inquilino.Dni);
                command.Parameters.AddWithValue("@Telefono", inquilino.Telefono);
                command.Parameters.AddWithValue("@Email", inquilino.Email);
                command.Parameters.AddWithValue("@Estado", inquilino.Estado);

                connection.Open();
                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }
    }

    public bool EliminarInquilino(int inquilinoId)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"DELETE FROM inquilinos 
                         WHERE {nameof(Inquilino.InquilinoID)} = @InquilinoID;";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@InquilinoID", inquilinoId);

                connection.Open();
                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }
    }

    public async Task<IList<Inquilino>> BuscarEnVivo(string term)
{
    List<Inquilino> resultados = new List<Inquilino>();
    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
    {
        string sql = @"SELECT InquilinoID, Nombre, Apellido, Dni, Telefono, Email, Estado
                       FROM inquilinos
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
                    var inquilino = new Inquilino
                    {
                        InquilinoID = reader.GetInt32(reader.GetOrdinal(nameof(Inquilino.InquilinoID))),
                            Nombre = reader.GetString(reader.GetOrdinal(nameof(Inquilino.Nombre))),
                            Apellido = reader.GetString(reader.GetOrdinal(nameof(Inquilino.Apellido))),
                            Dni = reader.GetInt32(reader.GetOrdinal(nameof(Inquilino.Dni))),
                            Email = reader.GetString(reader.GetOrdinal(nameof(Inquilino.Email))),
                            Telefono = reader.GetString(reader.GetOrdinal(nameof(Inquilino.Telefono))),
                            Estado = reader.GetBoolean(reader.GetOrdinal(nameof(Inquilino.Estado)))
                    };
                    resultados.Add(inquilino);
                }
            }
        }
    }
    return resultados;
}
}
