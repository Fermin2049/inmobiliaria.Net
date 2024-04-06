using MySql.Data.MySqlClient;

namespace asp.net.Models;

public class ControllerPropietario
{
    readonly string ConnectionString = "Server=localhost;Database=inmobiliaria;User=root;Password=;";

    public List<Propietario> GetPropietarios(){
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
                            Telefono = reader.GetInt32(nameof(Propietario.Telefono)),
                            Dni = reader.GetInt32(nameof(Propietario.Dni)),
                        });
                    }
                }
            }
            

        }
        return propietarios;
    }

    
}
