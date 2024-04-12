using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace asp.net.Models;

public class ControllerInmueble
{
    readonly string ConnectionString = "Server=localhost;Database=inmobiliaria;User=root;Password=;";

    public IList<Inmueble> GetInmuebles()
    {
        List<Inmueble> inmuebles = new List<Inmueble>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"SELECT {nameof(Inmueble.InmuebleID)}, 
                                {nameof(Inmueble.PropietarioID)}, 
                                {nameof(Inmueble.DireccionInmueble)}, 
                                {nameof(Inmueble.Uso)}, 
                                {nameof(Inmueble.Tipo)}, 
                                {nameof(Inmueble.CantAmbiente)}, 
                                {nameof(Inmueble.Valor)}, 
                                {nameof(Inmueble.Estado)} 
                         FROM inmuebles";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        inmuebles.Add(new Inmueble
                        {
                            InmuebleID = reader.GetInt32(reader.GetOrdinal(nameof(Inmueble.InmuebleID))),
                            PropietarioID = reader.GetInt32(reader.GetOrdinal(nameof(Inmueble.PropietarioID))),
                            DireccionInmueble = reader.GetString(reader.GetOrdinal(nameof(Inmueble.DireccionInmueble))),
                            Uso = reader.GetString(reader.GetOrdinal(nameof(Inmueble.Uso))),
                            Tipo = reader.GetString(reader.GetOrdinal(nameof(Inmueble.Tipo))),
                            CantAmbiente = reader.GetInt32(reader.GetOrdinal(nameof(Inmueble.CantAmbiente))),
                            Valor = reader.GetInt32(reader.GetOrdinal(nameof(Inmueble.Valor))),
                            Estado = reader.GetBoolean(reader.GetOrdinal(nameof(Inmueble.Estado)))
                        });
                    }
                }
            }
        }
        return inmuebles;
    }

    public int CrearInmueble(Inmueble inmueble)
    {
        int id = 0;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"INSERT INTO inmuebles 
                            ({nameof(Inmueble.PropietarioID)}, 
                             {nameof(Inmueble.DireccionInmueble)}, 
                             {nameof(Inmueble.Uso)}, 
                             {nameof(Inmueble.Tipo)}, 
                             {nameof(Inmueble.CantAmbiente)}, 
                             {nameof(Inmueble.Valor)}, 
                             {nameof(Inmueble.Estado)}) 
                         VALUES (@PropietarioID, @DireccionInmueble, @Uso, @Tipo, @CantAmbiente, @Valor, @Estado); 
                         SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@PropietarioID", inmueble.PropietarioID);
                command.Parameters.AddWithValue("@DireccionInmueble", inmueble.DireccionInmueble);
                command.Parameters.AddWithValue("@Uso", inmueble.Uso);
                command.Parameters.AddWithValue("@Tipo", inmueble.Tipo);
                command.Parameters.AddWithValue("@CantAmbiente", inmueble.CantAmbiente);
                command.Parameters.AddWithValue("@Valor", inmueble.Valor);
                command.Parameters.AddWithValue("@Estado", inmueble.Estado);

                connection.Open();
                id = Convert.ToInt32(command.ExecuteScalar());
            }
        }
        return id;
    }

    public bool ActualizarInmueble(Inmueble inmueble)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"UPDATE inmuebles 
                         SET {nameof(Inmueble.PropietarioID)} = @PropietarioID, 
                             {nameof(Inmueble.DireccionInmueble)} = @DireccionInmueble, 
                             {nameof(Inmueble.Uso)} = @Uso, 
                             {nameof(Inmueble.Tipo)} = @Tipo, 
                             {nameof(Inmueble.CantAmbiente)} = @CantAmbiente, 
                             {nameof(Inmueble.Valor)} = @Valor, 
                             {nameof(Inmueble.Estado)} = @Estado 
                         WHERE {nameof(Inmueble.InmuebleID)} = @InmuebleID;";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@InmuebleID", inmueble.InmuebleID);
                command.Parameters.AddWithValue("@PropietarioID", inmueble.PropietarioID);
                command.Parameters.AddWithValue("@DireccionInmueble", inmueble.DireccionInmueble);
                command.Parameters.AddWithValue("@Uso", inmueble.Uso);
                command.Parameters.AddWithValue("@Tipo", inmueble.Tipo);
                command.Parameters.AddWithValue("@CantAmbiente", inmueble.CantAmbiente);
                command.Parameters.AddWithValue("@Valor", inmueble.Valor);
                command.Parameters.AddWithValue("@Estado", inmueble.Estado);

                connection.Open();
                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }
    }

    public bool EliminarInmueble(int inmuebleId)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"DELETE FROM inmuebles 
                         WHERE {nameof(Inmueble.InmuebleID)} = @InmuebleID;";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@InmuebleID", inmuebleId);

                connection.Open();
                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }
    }
}
