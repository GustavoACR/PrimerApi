namespace PrimerApi;

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Contraseña { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
}
