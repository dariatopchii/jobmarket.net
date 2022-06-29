using System;
using System.Linq;
using JobMarket.Files.Workers;
using JobMarket.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobMarket.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IGenericCollection<UserModel> _collection;
        public UserController(IGenericCollection<UserModel> collection)
        {
            _collection = collection;
            _collection.StoragePath = "./Files/Settings/UserDirectory.json";
            _collection.ReadFromFile();
        }

        // POST api/values
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody]UserLoginModel request)
        {
            var log = _collection.GetByCondition(x => 
                    x.Email.Equals(request.Email)
                    && x.Password.Equals(request.Password))
                    .FirstOrDefault();

            if (log == null)
            {
                return NotFound();
            }

            return Ok(log);
        }

        // POST api/values/
        [HttpPost]
        [Route("Signup")]
        public IActionResult Register([FromBody]UserRequestModel request)
        { 
            var user = _collection.GetByCondition(u => u.Email == request.Email).FirstOrDefault();
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
                _collection.Create(newUser);
                _collection.Upload();
                return Ok(newUser);
            }
            catch (Exception)
            {
                return BadRequest(400);
            }
        }
        
    }
}

