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
        // API para editar una bebida existente: PUT /api/bebidas/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBebida(int id, Bebida bebida)
    {
        if (id != bebida.Id)
        {
            return BadRequest("El ID de la URL no coincide con el ID del cuerpo.");
        }

        _context.Entry(bebida).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Bebidas.Any(e => e.Id == id))
            {
                return NotFound($"No se encontró la bebida con ID {id}.");
            }
            else
            {
                throw;
            }
        }

        return Ok(bebida); // Devuelve el objeto editado con estado 200 OK
    }

    // API para eliminar una bebida: DELETE /api/bebidas/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBebida(int id)
    {
        var bebida = await _context.Bebidas.FindAsync(id);
        if (bebida == null)
        {
            return NotFound($"No se encontró la bebida con ID {id}.");
        }

        _context.Bebidas.Remove(bebida);
        await _context.SaveChangesAsync();

        return Ok(new { message = $"Bebida con ID {id} eliminada correctamente." });
    }


}
