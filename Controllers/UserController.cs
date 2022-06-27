using System;
using System.Linq;
using JobMarket.Files.Interfaces;
using JobMarket.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobMarket.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IStorageWorker<UserModel> _storage;

        public UserController(IStorageWorker<UserModel> storage)
        {
            _storage = storage;
        }

        // POST api/values
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody]UserLoginModel request)
        {
            var log = _storage.GetByCondition(x => x.Email.Equals(request.Email) && x.Password.Equals(request.Password)).FirstOrDefault();

            if (log == null)
            {
                return NotFound();
            }

            return Ok(log);
        }

        // POST api/values/
        [HttpPost]
        [Route("Signup")]
        public IActionResult InsertEmployee([FromBody]UserRequestModel request)
        {


            var user = _storage.GetByCondition(u => u.Email == request.Email).FirstOrDefault();
                if (user is not null)
                {
                    BadRequest(400);
                }

                try
                {
                    var newUser = new UserModel
                    {
                        Id = Guid.NewGuid(),
                        Email = request.Email,
                        Password = request.Password,
                        Name = request.Name
                    };
                    _storage.Create(newUser);
                    return Ok(newUser);
                }
                catch (Exception)
                {
                    return BadRequest(400);
                }
        }
    }
}

