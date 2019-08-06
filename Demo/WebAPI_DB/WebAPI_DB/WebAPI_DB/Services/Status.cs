using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_DB.Controllers;
using WebAPI_DB.Models;

namespace WebAPI_DB.Services
{
    public class Status : ControllerBase
    {
        private static bool UpdateDatabase = false;
        private ISession _session;

        public ISession Session { get { return _session; } }

        public Status(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public IEnumerable<Login> Read()
        {
            return GetAll();
        }
       
        public IList<Login> GetAll()
        {
            using (var _context = new dataContext())
            {
                var result = Session.GetObjectFromJson<IList<Login>>("Data");
                if (result == null || UpdateDatabase)
                {
                    result = _context.Login.ToList();
                    Session.SetObjectAsJson("Data", result);
                }
                return result;
            }
        }

        public void Create(Login login)
        {
            if (!UpdateDatabase)
            {
                var users = GetAll();
                var first = users.OrderByDescending(e => e.Id).FirstOrDefault();
                var id = (first != null) ? first.Id : 0;

                login.Id = id + 1;

                users.Insert(0, login);

                Session.SetObjectAsJson("Data", users);
            }
            else
            {
                using (var _context = new dataContext())
                {
                    var entity = new Login();

                    entity.Id = login.Id;
                    entity.Username = login.Username;                                        
                    _context.Login.Add(entity);
                    _context.SaveChanges();

                    login.Id = (int)entity.Id;
                }
            }
        }


        public void Update(Login login)
        {
            if (!UpdateDatabase)
            {
                var users = GetAll();
                var target = users.FirstOrDefault(e => e.Id == login.Id);

                if (target != null)
                {
                    target.Id = login.Id;
                    target.Username = login.Username;
                    
                }
                Session.SetObjectAsJson("Data", users);
            }
            else
            {
                using (var _context = new dataContext())
                {
                    var entity = new Login();

                    entity.Id = login.Id;
                    entity.Username = login.Username;                                       
                    _context.Login.Attach(entity);
                    _context.Entry(entity).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }
        }



        public IActionResult Destroy(Login login)
        {
            if (!UpdateDatabase)
            {
                var users = GetAll();
                var target = users.FirstOrDefault(e => e.Id == login.Id);

                if (target != null)
                {
                    users.Remove(target);
                }

                Session.SetObjectAsJson("Data", users);
            }
            else
            {
                using (var _context = new dataContext())
                {
                    var entity = new Login();

                    entity.Id = login.Id;

                    _context.Login.Attach(entity);

                    _context.Login.Remove(entity);
               
                    _context.SaveChanges();
                }
            }
            return NoContent();
        }


    }
}
