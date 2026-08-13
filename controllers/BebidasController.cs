using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PrimerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BebidasController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BebidasController(ApplicationDbContext context)
    {
        _context = context;
    }

    // API para obtener todos los cafés y postres: GET /api/bebidas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Bebida>>> GetBebidas()
    {
        return await _context.Bebidas.ToListAsync();
    }

    // API para registrar un nuevo producto: POST /api/bebidas
    [HttpPost]
    public async Task<ActionResult<Bebida>> PostBebida(Bebida bebida)
    {
        _context.Bebidas.Add(bebida);
        await _context.SaveChangesAsync();
        
        // Al usar Ok, devolvemos el producto creado de inmediato sin buscar otras rutas
        return Ok(bebida);
    }

}
