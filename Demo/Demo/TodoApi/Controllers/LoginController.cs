using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.Login;

namespace WebAPI.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class LoginController : Controller
    {
        private User[] users = new User[]
        {
            new User
                {
                    Id = 1,
                    Username = "thien",
                    Password = "123"
                },
            new User
                {
                    Id = 2,
                    Username = "aghdajsgdj",
                    Password = "11011994"
                }
    };
        

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IEnumerable<ActionResult<User>> Get(User f)
        {
            var display = users.Where(n => n.Username == f.Username && n.Password == f.Password).FirstOrDefault();
            yield return display;
        }


    }
}