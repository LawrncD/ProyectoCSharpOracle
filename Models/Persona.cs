namespace MiProyectoCSharp.Models;

public class Persona
{
    // Propiedades
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public int Edad { get; set; }

    // Constructor vacío
    public Persona() { }

    // Constructor con parámetros
    public Persona(string nombre, string apellido, int edad)
    {
        Nombre = nombre;
        Apellido = apellido;
        Edad = edad;
    }

    // Método
    public string NombreCompleto()
    {
        return $"{Nombre} {Apellido}";
    }

    // Override de ToString
    public override string ToString()
    {
        return $"{NombreCompleto()} - {Edad} años";
    }
}
