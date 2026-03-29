using System;

namespace MiProyectoCSharp.Models
{
    public class Jugador
    {
        public int IdJugador { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string? Posicion { get; set; }
        public decimal Peso { get; set; }
        public decimal Estatura { get; set; }
        public decimal ValorMercado { get; set; }
        public int IdEquipo { get; set; }
    }
}
