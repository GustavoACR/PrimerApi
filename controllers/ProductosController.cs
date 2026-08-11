using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PrimerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // API para obtener todos los cafés y postres: GET /api/productos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
    {
        return await _context.Productos.ToListAsync();
    }

    // API para registrar un nuevo producto: POST /api/productos
    // API para registrar un nuevo producto: POST /api/productos
    [HttpPost]
    public async Task<ActionResult<Producto>> PostProducto(Producto producto)
    {
        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();
        
        // Al usar Ok, devolvemos el producto creado de inmediato sin buscar otras rutas
        return Ok(producto);
    }

}
