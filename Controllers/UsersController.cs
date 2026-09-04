using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PrimerApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UsersController: ControllerBase
{
    private readonly  ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("Inseguro")]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuariosInseguro()
    {
        return await _context.Usuarios.ToListAsync();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetUsers()
    {
        var usuarios = await _context.Usuarios
            .Select(u => new 
            {
                u.Id,
                u.Nombre,
                u.Numero,
                u.Email,
                u.Rol
                // Aquí NO incluimos la propiedad Password o Contraseña
            })
            .ToListAsync();

        return Ok(usuarios);
    }

    // [HttpPost]
    // public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
    // {
    //     _context.Usuarios.Add(usuario);
    //     await _context.SaveChangesAsync();
        
    //     // Al usar Ok, devolvemos el producto creado de inmediato sin buscar otras rutas
    //     return Ok(usuario);
    // }

        // API para registrar un nuevo usuario: POST /api/users
    [HttpPost]
    public async Task<ActionResult<object>> PostUsuario(Usuario usuario)
    {
        // 1. Validar si el correo ya existe en la base de datos
        var existeUsuario = await _context.Usuarios.AnyAsync(u => u.Numero == usuario.Numero);
        if (existeUsuario)
        {
            return BadRequest(new { message = "El número de teléfono ya está registrado." });
        }

        // 2. Encriptar (Hash) la contraseña usando BCrypt (o la herramienta de tu elección)
        // Nota: Para este ejemplo usamos BCrypt.Net. Si no lo tienes, puedes instalarlo con:
        // dotnet add package BCrypt.Net-Next
        string contraseñaEncriptada = BCrypt.Net.BCrypt.HashPassword(usuario.Contraseña);

        // 3. Crear el objeto que se guardará en la base de datos
        var nuevoUsuario = new Usuario
        {
            Nombre = usuario.Nombre,
            Numero = usuario.Numero,
            Email = usuario.Email,
            Rol = usuario.Rol,
            Contraseña = contraseñaEncriptada // Guardamos el texto encriptado irreversible
        };

        // 4. Guardar en la base de datos
        _context.Usuarios.Add(nuevoUsuario);
        await _context.SaveChangesAsync();

        // 5. Responder al frontend ocultando la contraseña por seguridad
        return Ok(new
        {
            Id = nuevoUsuario.Id,
            Nombre = nuevoUsuario.Nombre,
            Numero = nuevoUsuario.Numero,
            Email = nuevoUsuario.Email,
            Rol = nuevoUsuario.Rol,
            Mensaje = "Usuario registrado exitosamente."
        });
    }

    // [HttpPut("{id}")]
    // public async Task<IActionResult> PutUser(int id, Usuario usuario)
    // {
    //     if (id != usuario.Id)
    //     {
    //         return BadRequest(new { message = "El ID de la URL no coincide con el ID del cuerpo." });
    //     }

    //     _context.Entry(usuario).State = EntityState.Modified;

    //     try
    //     {
    //         await _context.SaveChangesAsync();
    //     }
    //     catch (DbUpdateConcurrencyException)
    //     {
    //         if (!_context.Usuarios.Any(e => e.Id == id))
    //         {
    //             return NotFound(new { message = $"No se encontró el usuario con ID {id}." });
    //         }
    //         else
    //         {
    //             throw;
    //         }
    //     }

    //     return Ok(usuario); // Devuelve el objeto editado con estado 200 OK
    // }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, Usuario usuarioEditado)
    {
        if (id != usuarioEditado.Id)
        {
            return BadRequest(new { message = "El ID de la URL no coincide con el ID del cuerpo." });
        }

        // 1. Buscamos el usuario real que está guardado actualmente
        var usuarioDb = await _context.Usuarios.FindAsync(id);
        if (usuarioDb == null)
        {
            return NotFound(new { message = $"No se encontró el usuario con ID {id}." });
        }

        // 2. Modificamos SOLO los campos permitidos (La contraseña NO se toca aquí)
        usuarioDb.Nombre = usuarioEditado.Nombre;
        usuarioDb.Numero = usuarioEditado.Numero;
        usuarioDb.Email = usuarioEditado.Email;
        usuarioDb.Rol = usuarioEditado.Rol;
        // u otros campos como Email, etc.

        // 1. Validar si el correo ya existe en la base de datos
        var existeUsuario = await _context.Usuarios.AnyAsync(u => u.Numero == usuarioEditado.Numero);
        if (existeUsuario)
        {
            return BadRequest(new { message = "El número de teléfono ya está registrado." });
        }

        try
        {
            // 3. Guardamos los cambios (EF Core detecta automáticamente qué cambió)
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Usuarios.Any(e => e.Id == id))
            {
                return NotFound(new { message = $"No se encontró el usuario con ID {id}." });
            }
            else
            {
                throw;
            }
        }

        // 4. Respondemos con un objeto limpio, protegiendo los datos sensibles
        return Ok(new
        {
            id = usuarioDb.Id,
            nombre = usuarioDb.Nombre,
            numero = usuarioDb.Numero,
            email = usuarioDb.Email,
            rol = usuarioDb.Rol,
            message = "Usuario actualizado con éxito."
        });
    }


    // API para eliminar una bebida: DELETE /api/bebidas/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound(new { message = $"No se encontró el usuario con ID {id}." });
        }

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        return Ok(new { message = $"Bebida con ID {id} eliminada correctamente." });
    }

}