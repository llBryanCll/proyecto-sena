using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoSenaLmour.Models;

namespace ProyectoSenaLmour.Controllers
{
    public class ClientesController : Controller
    {
        private readonly LmourContext _context;

        public ClientesController(LmourContext context)
        {
            _context = context;
        }

        // Método para cambiar el estado del cliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int? id, string nuevoEstado)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            // Cambiar el estado del cliente
            cliente.Estado = nuevoEstado;

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Redirigir de vuelta a la vista de índice
            return RedirectToAction(nameof(Index));
        }


        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            var lmourContext = _context.Clientes.Include(c => c.IdRolNavigation).Include(c => c.IdTipoDocumentoNavigation);
            return View(await lmourContext.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.IdRolNavigation)
                .Include(c => c.IdTipoDocumentoNavigation)
                .FirstOrDefaultAsync(m => m.NroDocumento == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "IdRol");
            ViewData["IdTipoDocumento"] = new SelectList(_context.TipoDocumentos, "IdTipoDocumento", "IdTipoDocumento");
            ViewData["Generos"] = new SelectList(new[] { "Masculino", "Femenino", "Prefiero no decirlo" });
            ViewData["Estados"] = new SelectList(new[] { "Activo", "Inactivo" });
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NroDocumento,IdTipoDocumento,Nombres,Apellidos,Celular,Correo,Contraseña,Telefono,Direccion,Genero,Estado,IdRol")] Cliente cliente)
        {
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "IdRol", cliente.IdRol);
            ViewData["IdTipoDocumento"] = new SelectList(_context.TipoDocumentos, "IdTipoDocumento", "IdTipoDocumento", cliente.IdTipoDocumento);
            ViewData["Generos"] = new SelectList(new[] { "Masculino", "Femenino", "Prefiero no decirlo" });
            ViewData["Estados"] = new SelectList(new[] { "Activo", "Inactivo" });

            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }
        // POST: Clientes/Delete/5
    
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "IdRol", cliente.IdRol);
            ViewData["IdTipoDocumento"] = new SelectList(_context.TipoDocumentos, "IdTipoDocumento", "IdTipoDocumento", cliente.IdTipoDocumento);
            ViewData["Generos"] = new SelectList(new[] { "Masculino", "Femenino", "Prefiero no decirlo" });
            ViewData["Estados"] = new SelectList(new[] { "Activo", "Inactivo" });

            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NroDocumento,IdTipoDocumento,Nombres,Apellidos,Celular,Correo,Contraseña,Telefono,Direccion,Genero,Estado,IdRol")] Cliente cliente)
        {
            if (id != cliente.NroDocumento)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (ClienteExists(cliente.NroDocumento))
                    {
                        throw;
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }

        private bool ClienteExists(int nroDocumento)
        {
            throw new NotImplementedException();
        }
        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.NroDocumento == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}


