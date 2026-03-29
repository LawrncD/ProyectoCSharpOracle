namespace MiProyectoCSharp.Models
{
    public class Estadio
    {
        public int IdEstadio { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Capacidad { get; set; }
        public int IdCiudad { get; set; }
    }
}
