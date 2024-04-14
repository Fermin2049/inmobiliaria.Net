using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace asp.net.Models;

public class RepositorioPago
{
    readonly string ConnectionString = "Server=localhost;Database=inmobiliaria;User=root;Password=;";

    public IList<Pago> GetPagos()
    {
        List<Pago> pagos = new List<Pago>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"SELECT {nameof(Pago.PagoID)}, 
                                {nameof(Pago.ContratoID)}, 
                                {nameof(Pago.NroPago)}, 
                                {nameof(Pago.FechaPago)}, 
                                {nameof(Pago.Detalle)}, 
                                {nameof(Pago.Importe)}, 
                                {nameof(Pago.Estado)} 
                         FROM pagos";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pagos.Add(new Pago
                        {
                            PagoID = reader.GetInt32(reader.GetOrdinal(nameof(Pago.PagoID))),
                            ContratoID = reader.GetInt32(reader.GetOrdinal(nameof(Pago.ContratoID))),
                            NroPago = reader.GetInt32(reader.GetOrdinal(nameof(Pago.NroPago))),
                            FechaPago = reader.GetDateTime(reader.GetOrdinal(nameof(Pago.FechaPago))),
                            Detalle = reader.GetString(reader.GetOrdinal(nameof(Pago.Detalle))),
                            Importe = reader.GetDecimal(reader.GetOrdinal(nameof(Pago.Importe))),
                            Estado = reader.GetBoolean(reader.GetOrdinal(nameof(Pago.Estado)))
                        });
                    }
                }
            }
        }
        return pagos;
    }

    public int CrearPago(Pago pago)
    {
        int id = 0;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"INSERT INTO pagos 
                            ({nameof(Pago.ContratoID)}, 
                             {nameof(Pago.NroPago)}, 
                             {nameof(Pago.FechaPago)}, 
                             {nameof(Pago.Detalle)}, 
                             {nameof(Pago.Importe)}, 
                             {nameof(Pago.Estado)}) 
                         VALUES (@ContratoID, @NroPago, @FechaPago, @Detalle, @Importe, @Estado); 
                         SELECT LAST_INSERT_ID();";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@ContratoID", pago.ContratoID);
                command.Parameters.AddWithValue("@NroPago", pago.NroPago);
                command.Parameters.AddWithValue("@FechaPago", pago.FechaPago);
                command.Parameters.AddWithValue("@Detalle", pago.Detalle);
                command.Parameters.AddWithValue("@Importe", pago.Importe);
                command.Parameters.AddWithValue("@Estado", pago.Estado);

                connection.Open();
                id = Convert.ToInt32(command.ExecuteScalar());
            }
        }
        return id;
    }

    public bool ActualizarPago(Pago pago)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"UPDATE pagos 
                         SET {nameof(Pago.ContratoID)} = @ContratoID, 
                             {nameof(Pago.NroPago)} = @NroPago, 
                             {nameof(Pago.FechaPago)} = @FechaPago, 
                             {nameof(Pago.Detalle)} = @Detalle, 
                             {nameof(Pago.Importe)} = @Importe, 
                             {nameof(Pago.Estado)} = @Estado 
                         WHERE {nameof(Pago.PagoID)} = @PagoID;";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@PagoID", pago.PagoID);
                command.Parameters.AddWithValue("@ContratoID", pago.ContratoID);
                command.Parameters.AddWithValue("@NroPago", pago.NroPago);
                command.Parameters.AddWithValue("@FechaPago", pago.FechaPago);
                command.Parameters.AddWithValue("@Detalle", pago.Detalle);
                command.Parameters.AddWithValue("@Importe", pago.Importe);
                command.Parameters.AddWithValue("@Estado", pago.Estado);

                connection.Open();
                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }
    }

    public bool EliminarPago(int pagoId)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"DELETE FROM pagos 
                         WHERE {nameof(Pago.PagoID)} = @PagoID;";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@PagoID", pagoId);

                connection.Open();
                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }
    }
}
