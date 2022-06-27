using System;
using System.Collections.Generic;
using System.Linq;
using JobMarket.Files.Interfaces;
using JobMarket.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobMarket.Controllers
{
    [Route("api/[controller]")]
    public class CvController : Controller
    {
        // GET: /<controller>/
        
        private readonly IStorageWorker<CvModel> _storage;
        public CvController(IStorageWorker<CvModel> storage)
        {
            _storage = storage;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public CvModel GetCvById(Guid id)
        {

            return (_storage.GetByCondition(cv => cv.Id == id)).FirstOrDefault();

        }
        
        // GET: api/values
        [HttpGet]
        public IActionResult GetCvs()
        {
            List<CvModel> cvs = _storage.GetAll().ToList();
            return Ok(cvs);
        }
        
        [HttpGet]
        [Route("UserCvs")]
        public IActionResult GetUserCvs(bool arch, Guid userId)
        {
            try
            {
                List<CvModel> cvs = (_storage.GetByCondition(u => u.UserId == userId && u.IsArchived == arch)).ToList();

                return Ok(cvs);
            }
            catch
            {
                return new BadRequestResult();
            }
        }

        // GET: api/values
        [HttpPost]
        [Route("FilterCvs")]
        public IActionResult FilterCvs([FromBody]FilterModel filter)
        {
            var cvs = _storage.GetByCondition(cv =>
                (string.IsNullOrEmpty(filter.Occupation) || cv.Occupation == filter.Occupation)
                &&(string.IsNullOrEmpty(filter.Name) || cv.Name == filter.Name)
                && (string.IsNullOrEmpty(filter.Location) || cv.Location == filter.Location)
                && (!filter.MinSalary.HasValue || cv.Salary >= filter.MinSalary)
                && (!filter.MaxSalary.HasValue || cv.Salary <= filter.MinSalary)
                &&(cv.IsArchived == false)
            ).ToList();
            return Ok(cvs);
        }

        // POST api/values
        [HttpPost]
        public IActionResult CreateCv([FromBody]CvRequestModel cv)
        {
            try
            {
                var newCv = new CvModel
                {
                    Id = Guid.NewGuid(),
                    Email = cv.Email,
                    Name = cv.Name,
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
                    IsArchived = cv.IsArchived
                };
                _storage.Create(newCv);
                return Ok();
            }
            catch
            {
                return BadRequest(400);
            }


        }
        

        // PUT api/values/5
        [HttpPut]
        public void EditCv([FromBody] CvRequestModel cv)
        {
            var oldCv = _storage.GetByCondition(u => u.UserId == cv.UserId).FirstOrDefault();
            _storage.Delete(oldCv);
            var newCv = new CvModel
            {
                Id = Guid.NewGuid(),
                Email = cv.Email,
                Name = cv.Name,
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
                IsArchived = cv.IsArchived
            };
            _storage.Create(newCv);
        }
        
        // PUT api/values/5
        [Route("Archive")]
        [HttpPut]
        public IActionResult Archive([FromBody]BaseModel id)
        {
            var cv = _storage.GetByCondition(cv => cv.Id == id.Id).FirstOrDefault();
            var newCv = new CvModel
                {
                    Id = Guid.NewGuid(),
                    Email = cv.Email,
                    Name = cv.Name,
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
                    IsArchived = !cv.IsArchived
                };
                _storage.Delete(cv);
                _storage.Create(newCv);
                return Ok(200);
            
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCv(Guid id)
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

