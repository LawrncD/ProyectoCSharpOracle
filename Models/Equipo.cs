namespace MiProyectoCSharp.Models
{
    public class Equipo
    {
        public int IdEquipo { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public int IdConfederacion { get; set; }
        public decimal ValorTotalEquipo { get; set; }
    }
}
