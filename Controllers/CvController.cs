using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobMarket.Files.Interfaces;
using JobMarket.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JobMarket.Controllers
{
    [Route("api/[controller]")]
    public class CvController : Controller
    {
        // GET: /<controller>/

        private readonly IGenericStorageWorker<CvModel> _storage;

        public CvController(IGenericStorageWorker<CvModel> storage)
        {
            _storage = storage;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public CvModel GetCvById(Guid id)
        {

            return (_storage.GetByCondition(u => u.Id == id)).FirstOrDefault();

        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<CvModel> Get()
        {
            var vacancies = _storage.GetAll();
            return vacancies;
        }
        
        [HttpGet]
        [Route("UserCvs")]
        public IActionResult GetUserCvs(Guid userId)
        {
            List<CvModel> cvs = (_storage.GetByCondition(u => u.UserId == userId)).ToList();

            if (cvs.Any())
            {
                return new JsonResult(cvs);
            }

            return BadRequest();
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] CvRequestModel cv)
        {
            try
            {
                var newCv = new CvModel
                {
                    Id = Guid.NewGuid(),
                    Email = cv.Email,
                    Name = cv.Name,
                    Gender = cv.Gender,
                    Location = cv.Location,
                    Occupation = cv.Occupation,
                    Education = cv.Education,
                    Workplace = cv.Workplace,
                    Firm = cv.Firm,
                    Position = cv.Position,
                    Salary = cv.Salary,
                    Description = cv.Description,
                    Requirements = cv.Requirements,
                    UserId = cv.UserId,
                };
                _storage.Create(newCv);
                return Ok();
            }
            catch(Exception)
            {
                return BadRequest(400);
            }


        }

        // PUT api/values/5
        [HttpPut]
        public void Put(Guid id, [FromBody] CvModel cv)
        {
            var oldCv = _storage.GetByCondition(u => u.Id == id).FirstOrDefault();
            _storage.Delete(oldCv);
            _storage.Create(cv);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var cv = GetCvById(id);
            if (cv is null)
            {
                return NotFound();
            }

            _storage.Delete(cv);
            return Ok();

        }

    }
}

