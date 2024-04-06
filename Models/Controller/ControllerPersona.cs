using MySql.Data.MySqlClient;

namespace asp.net.Models;

public class ControllerPersona
{
    readonly string ConnectionString = "Server=localhost;Database=inmobiliaria;User=root;Password=;";

    public List<Persona> GetPersonas(){
        List<Persona> personas = new List<Persona>();
        using (MySqlConnection connection = new MySqlConnection(ConnectionString)){
            var sql = @$"SELECT {nameof(Persona.Id)}, {nameof(Persona.Nombre)}, {nameof(Persona.Apellido)}, {nameof(Persona.Dni)}, {nameof(Persona.Email)} FROM personas";
            using(var command = new MySqlCommand(sql, connection)){
                connection.Open();
                using(var reader = command.ExecuteReader()){
                    while(reader.Read()){
                        personas.Add(new Persona{
                            Id = reader.GetInt32(nameof(Persona.Id)),
                            Nombre = reader.GetString(nameof(Persona.Nombre)),
                            Apellido = reader.GetString(nameof(Persona.Apellido)),
                            Dni = reader.GetString(nameof(Persona.Dni)),
                            Email = reader.GetString(nameof(Persona.Email))
                        });
                    }
                }
            }
            

        }
        return personas;
    }
}
