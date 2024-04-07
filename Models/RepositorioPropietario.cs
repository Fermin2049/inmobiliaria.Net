using System.ComponentModel.Design;
using MySql.Data.MySqlClient;

namespace asp.net.Models;

public class ControllerPropietario
{
    readonly string ConnectionString = "Server=localhost;Database=inmobiliaria;User=root;Password=;";

    public IList<Propietario> GetPropietarios()
    {
        List<Propietario> propietarios = new List<Propietario>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString)){
            var sql = @$"SELECT {nameof(Propietario.PropietarioID)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Apellido)}, {nameof(Propietario.Email)}, {nameof(Propietario.Telefono)}, {nameof(Propietario.Dni)} FROM propietarios";
            using(var command = new MySqlCommand(sql, connection)){
                connection.Open();
                using(var reader = command.ExecuteReader()){
                    while(reader.Read()){
                        propietarios.Add(new Propietario{
                            PropietarioID = reader.GetInt32(nameof(Propietario.PropietarioID)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            Email = reader.GetString(nameof(Propietario.Email)),
                            Telefono = reader.GetString(nameof(Propietario.Telefono)),
                            Dni = reader.GetInt32(nameof(Propietario.Dni)),
                        });
                    }
                }
            }
            

        }
        return propietarios;
    }


    public int CrearPropietario(Propietario propietario){
        int id = 0;
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"INSERT INTO propietarios ({nameof(Propietario.Nombre)}, {nameof(Propietario.Apellido)}, {nameof(Propietario.Email)}, {nameof(Propietario.Telefono)}, {nameof(Propietario.Dni)})
            VALUES (@{nameof(Propietario.Nombre)}, @{nameof(Propietario.Apellido)}, @{nameof(Propietario.Email)}, @{nameof(Propietario.Telefono)}, @{nameof(Propietario.Dni)});
            SELECT LAST_INSERT_ID();";
            
            using(var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue(nameof(Propietario.Nombre), propietario.Nombre);
                command.Parameters.AddWithValue(nameof(Propietario.Apellido), propietario.Apellido);
                command.Parameters.AddWithValue(nameof(Propietario.Email), propietario.Email);
                command.Parameters.AddWithValue(nameof(Propietario.Telefono), propietario.Telefono);
                command.Parameters.AddWithValue(nameof(Propietario.Dni), propietario.Dni);

                connection.Open();
                id = Convert.ToInt32(command.ExecuteScalar());
                propietario.PropietarioID = id;
                connection.Close();
        }
        }
        return id;
    }

}
    