﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoSenaLmour.Models;

namespace ProyectoSenaLmour.Controllers
{

    //[Authorize(Roles = "administrador")]
    public class UsuariosController : Controller
    {
        private readonly LmourContext _context;

        public UsuariosController(LmourContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var lmourContext = _context.Usuarios.Include(u => u.IdRolNavigation);
            return View(await lmourContext.ToListAsync());
        }


        //public IActionResult CambiarEstado(int id, string nuevoEstado)
        //{
        //    // Update the user's state in your data store (e.g., database)
        //    Usuario.UpdateEstado(id, nuevoEstado);

        //    // Optional: Return a success message or redirect to another action

        //    return Ok(); // Or other appropriate response
        //}





		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CambiarEstado(int? id, string nuevoEstado)
		{
			if (id == null)
			{
				return NotFound();
			}

			var Usuarios = await _context.Clientes.FindAsync(id);
			if (Usuarios == null)
			{
				return NotFound();
			}

			// Cambiar el estado del cliente
			Usuarios.Estado = nuevoEstado;

			// Guardar los cambios en la base de datos
			await _context.SaveChangesAsync();

			// Redirigir de vuelta a la vista de índice
			return RedirectToAction(nameof(Index));
		}





		// GET: Usuarios/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.NroDocumento == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "IdRol");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NroDocumento,IdTipoDocumento,Nombres,Apellidos,Celular,Correo,Contraseña,Telefono,Direccion,Genero,Estado,IdRol")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "IdRol", usuario.IdRol);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "IdRol", usuario.IdRol);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NroDocumento,IdTipoDocumento,Nombres,Apellidos,Celular,Correo,Contraseña,Telefono,Direccion,Genero,Estado,IdRol")] Usuario usuario)
        {
            if (id != usuario.NroDocumento)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.NroDocumento))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "IdRol", usuario.IdRol);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.NroDocumento == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'LmourContext.Usuarios'  is null.");
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
          return (_context.Usuarios?.Any(e => e.NroDocumento == id)).GetValueOrDefault();
        }
    }

    public static class EstadoHelper
    {
        public static string MostrarEstado(bool estado)
        {
            if (estado)
            {
                return "Activado";
            }
            else
            {
                return "Inactivado";
            }
        }
    }



}
