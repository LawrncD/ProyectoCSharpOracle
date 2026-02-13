namespace MiProyectoCSharp.Models;

// Herencia: Estudiante hereda de Persona
public class Estudiante : Persona
{
    public string Carrera { get; set; }
    public double Promedio { get; set; }

    public Estudiante() { }

    // Usa 'base' para llamar al constructor del padre
    public Estudiante(string nombre, string apellido, int edad, string carrera, double promedio)
        : base(nombre, apellido, edad)
    {
        Carrera = carrera;
        Promedio = promedio;
    }

    public bool EstaAprobado()
    {
        return Promedio >= 6.0;
    }

    public override string ToString()
    {
        string estado = EstaAprobado() ? "Aprobado" : "Reprobado";
        return $"{NombreCompleto()} | Carrera: {Carrera} | Promedio: {Promedio:F1} ({estado})";
    }
}
