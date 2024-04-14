using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace asp.net.Models;

public class RepositorioContrato
{
    readonly string ConnectionString = "Server=localhost;Database=inmobiliaria;User=root;Password=;";

    public IList<Contrato> GetContratos()
    {
        List<Contrato> contratos = new List<Contrato>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"SELECT {nameof(Contrato.ContratoID)}, 
                                {nameof(Contrato.InmuebleID)}, 
                                {nameof(Contrato.InquilinoID)}, 
                                {nameof(Contrato.FechaInicio)}, 
                                {nameof(Contrato.FechaFin)}, 
                                {nameof(Contrato.MontoRenta)}, 
                                {nameof(Contrato.Deposito)}, 
                                {nameof(Contrato.Comision)}, 
                                {nameof(Contrato.Condiciones)} 
                         FROM contratos";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        contratos.Add(new Contrato
                        {
                            ContratoID = reader.GetInt32(reader.GetOrdinal(nameof(Contrato.ContratoID))),
                            InmuebleID = reader.GetInt32(reader.GetOrdinal(nameof(Contrato.InmuebleID))),
                            InquilinoID = reader.GetInt32(reader.GetOrdinal(nameof(Contrato.InquilinoID))),
                            FechaInicio = reader.GetDateTime(reader.GetOrdinal(nameof(Contrato.FechaInicio))),
                            FechaFin = reader.GetDateTime(reader.GetOrdinal(nameof(Contrato.FechaFin))),
                            MontoRenta = reader.GetInt32(reader.GetOrdinal(nameof(Contrato.MontoRenta))),
                            Deposito = reader.GetInt32(reader.GetOrdinal(nameof(Contrato.Deposito))),
                            Comision = reader.GetInt32(reader.GetOrdinal(nameof(Contrato.Comision))),
                            Condiciones = reader.IsDBNull(reader.GetOrdinal(nameof(Contrato.Condiciones))) ? null : reader.GetString(reader.GetOrdinal(nameof(Contrato.Condiciones)))
                        });
                    }
                }
            }
        }
        return contratos;
    }

    public int CrearContrato(Contrato contrato)
    {
        int id = 0;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"INSERT INTO contratos 
                            ({nameof(Contrato.InmuebleID)}, 
                             {nameof(Contrato.InquilinoID)}, 
                             {nameof(Contrato.FechaInicio)}, 
                             {nameof(Contrato.FechaFin)}, 
                             {nameof(Contrato.MontoRenta)}, 
                             {nameof(Contrato.Deposito)}, 
                             {nameof(Contrato.Comision)}, 
                             {nameof(Contrato.Condiciones)}) 
                         VALUES (@InmuebleID, @InquilinoID, @FechaInicio, @FechaFin, @MontoRenta, @Deposito, @Comision, @Condiciones); 
                         SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@InmuebleID", contrato.InmuebleID);
                command.Parameters.AddWithValue("@InquilinoID", contrato.InquilinoID);
                command.Parameters.AddWithValue("@FechaInicio", contrato.FechaInicio);
                command.Parameters.AddWithValue("@FechaFin", contrato.FechaFin);
                command.Parameters.AddWithValue("@MontoRenta", contrato.MontoRenta);
                command.Parameters.AddWithValue("@Deposito", contrato.Deposito);
                command.Parameters.AddWithValue("@Comision", contrato.Comision);
                command.Parameters.AddWithValue("@Condiciones", contrato.Condiciones ?? (object)DBNull.Value);

                connection.Open();
                id = Convert.ToInt32(command.ExecuteScalar());
            }
        }
        return id;
    }

    public bool ActualizarContrato(Contrato contrato)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"UPDATE contratos 
                         SET {nameof(Contrato.InmuebleID)} = @InmuebleID, 
                             {nameof(Contrato.InquilinoID)} = @InquilinoID, 
                             {nameof(Contrato.FechaInicio)} = @FechaInicio, 
                             {nameof(Contrato.FechaFin)} = @FechaFin, 
                             {nameof(Contrato.MontoRenta)} = @MontoRenta, 
                             {nameof(Contrato.Deposito)} = @Deposito, 
                             {nameof(Contrato.Comision)} = @Comision, 
                             {nameof(Contrato.Condiciones)} = @Condiciones 
                         WHERE {nameof(Contrato.ContratoID)} = @ContratoID;";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@ContratoID", contrato.ContratoID);
                command.Parameters.AddWithValue("@InmuebleID", contrato.InmuebleID);
                command.Parameters.AddWithValue("@InquilinoID", contrato.InquilinoID);
                command.Parameters.AddWithValue("@FechaInicio", contrato.FechaInicio);
                command.Parameters.AddWithValue("@FechaFin", contrato.FechaFin);
                command.Parameters.AddWithValue("@MontoRenta", contrato.MontoRenta);
                command.Parameters.AddWithValue("@Deposito", contrato.Deposito);
                command.Parameters.AddWithValue("@Comision", contrato.Comision);
                command.Parameters.AddWithValue("@Condiciones", contrato.Condiciones ?? (object)DBNull.Value);

                connection.Open();
                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }
    }

    public bool EliminarContrato(int contratoId)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"DELETE FROM contratos 
                         WHERE {nameof(Contrato.ContratoID)} = @ContratoID;";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@ContratoID", contratoId);

                connection.Open();
                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }
    }
}
