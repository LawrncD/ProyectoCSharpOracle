using MiProyectoCSharp.Models;

// === PERSONA ===
var persona = new Persona("Juan", "Pérez", 30);
Console.WriteLine("=== Persona ===");
Console.WriteLine(persona);

// === ESTUDIANTE (herencia) ===
var estudiante1 = new Estudiante("María", "López", 22, "Ingeniería", 8.5);
var estudiante2 = new Estudiante("Carlos", "Gómez", 20, "Medicina", 5.2);

Console.WriteLine("\n=== Estudiantes ===");
Console.WriteLine(estudiante1);
Console.WriteLine(estudiante2);

// === PRODUCTO ===
var productos = new List<Producto>
{
    new Producto(1, "Laptop", 999.99m, 5),
    new Producto(2, "Mouse", 29.50m, 0),
    new Producto(3, "Teclado", 75.00m, 12)
};

Console.WriteLine("\n=== Productos ===");
foreach (var p in productos)
{
    Console.WriteLine($"{p} | Con IVA: ${p.PrecioConIva():F2} | Disponible: {p.HayStock()}");
}

// === LINQ básico ===
var conStock = productos.Where(p => p.HayStock()).ToList();
Console.WriteLine($"\nProductos con stock: {conStock.Count}");

var masBarato = productos.MinBy(p => p.Precio);
Console.WriteLine($"Más barato: {masBarato?.Nombre} (${masBarato?.Precio})");
