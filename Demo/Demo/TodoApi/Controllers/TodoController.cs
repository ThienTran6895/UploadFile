﻿#define Primary
#if Primary
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using WebAPI.Models.Upload;
using Microsoft.Extensions.FileProviders;
using System.Runtime.Serialization;
using System.Text;
using System.Runtime.Serialization.Json;
using TodoApi.Models;



#region TodoController
namespace TodoApi.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;
        #endregion

        public TodoController(TodoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                _context.TodoItems.AddRange(
                new TodoItem
                {
                    Username = "thien",
                    Password = "123"                    
                },
                new TodoItem
                {
                    Username = "uyen",
                    Password = "11011994"
                });
                _context.SaveChanges();
            }
        }

        #region snippet_GetAll
        // GET: api/Todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            
            return await _context.TodoItems.ToListAsync();

        }



        #region snippet_GetByID
        // GET: api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }
        #endregion
        #endregion

        #region snippet_Create
        // POST: api/Todo
        [HttpPost]
        public ActionResult<TodoItem> PostTodoItem(TodoItem item)
        {
            string sUsername = item.Username;
            string sPassword = item.Password;
            TodoItem todo = _context.TodoItems.SingleOrDefault(n => n.Username == sUsername && n.Password == sPassword);
            if (todo != null)
            {
                return RedirectToAction("Home.html");
            }
            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng!!!");
            return RedirectToAction("Login.html");
            //_context.TodoItems.Add(item);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
        }
        #endregion

        #region snippet_Update
        // PUT: api/Todo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        #region snippet_Delete
        // DELETE: api/Todo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion        

    }
}
#endif