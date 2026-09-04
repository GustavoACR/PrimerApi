namespace PrimerApi;

public class Bebida
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public string Descripcion { get; set; } = string.Empty; // Ej: "Bebidas", "Postres"
}
