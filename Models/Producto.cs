namespace MiProyectoCSharp.Models;

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }

    public Producto(int id, string nombre, decimal precio, int stock)
    {
        Id = id;
        Nombre = nombre;
        Precio = precio;
        Stock = stock;
    }

    public bool HayStock() => Stock > 0;

    public decimal PrecioConIva(decimal porcentajeIva = 0.21m)
    {
        return Precio * (1 + porcentajeIva);
    }

    public override string ToString()
    {
        return $"[{Id}] {Nombre} - ${Precio:F2} (Stock: {Stock})";
    }
}
