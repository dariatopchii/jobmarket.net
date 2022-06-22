using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobMarket.Files.Interfaces;
using JobMarket.Models;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JobMarket.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IGenericStorageWorker<UserModel> _storage;

        public UserController(IGenericStorageWorker<UserModel> storage)
        {
            _storage = storage;
        }

        // POST api/values
        [HttpGet]
        public Response Login(string email, string password)
        {
            var log = _storage.GetByCondition(x => x.Email.Equals(email) && x.Password.Equals(password)).FirstOrDefault();

            if (log == null)
            {
                return new Response { Status = "Invalid", Message = "Invalid Data." };
            }
            else

                return new Response { Status = "Success", Message = "Valid." };
        }

        // POST api/values/
        [HttpPost]
        public object InsertEmployee( string userName, string email, string password)
        {


            var user = _storage.GetByCondition(u => u.Email == email).FirstOrDefault();
                if (user is not null)
                {
                    throw new Exception("user already exists");
                }

                try
                {
                    var newUser = new UserModel();
                    newUser.Email = email;
                    newUser.Password = password;
                    newUser.Name = userName;
                    _storage.Create(newUser);
                    return new Response { Status = "Success", Message = "Valid" };
                }
                catch (Exception)
                {
                   
                    return new Response { Status = "Failure", Message = "InValid" };
                }
        }
    }
}

