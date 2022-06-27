using System;
using System.Collections.Generic;
using System.Linq;
using JobMarket.Files.Interfaces;
using JobMarket.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobMarket.Controllers
{
    [Route("api/[controller]")]
    public class VacancyController : Controller
    {
        // GET: /<controller>/

        private readonly IStorageWorker<VacancyModel> _storage;

        public VacancyController(IStorageWorker<VacancyModel> storage)
        {
            _storage = storage;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public VacancyModel GetVacancyById(Guid id)
        {

            return _storage.GetByCondition(vac => vac.Id == id).FirstOrDefault();

        }
        
        [HttpGet]
        public IActionResult GetVacs()
        {
            List<VacancyModel> vac = _storage.GetAll().ToList();
            return new JsonResult(vac);
        }

        // GET: api/values
        [HttpPost]
        [Route("FilterVacs")]
        public List<VacancyModel> GetVacs([FromBody]FilterModel filter)
        {
            var vacs = _storage.GetByCondition(vac =>
                (string.IsNullOrEmpty(filter.Occupation) || vac.Occupation == filter.Occupation)
                &&(string.IsNullOrEmpty(filter.Name) || vac.Name == filter.Name)
                && (string.IsNullOrEmpty(filter.Location) || vac.Location == filter.Location)
                && (!filter.MinSalary.HasValue || vac.Salary >= filter.MinSalary)
                && (!filter.MaxSalary.HasValue || vac.Salary <= filter.MinSalary)
                &&(vac.IsArchived == false)
            ).ToList();
            return vacs;
        }
        
        [HttpGet]
        [Route("UserVacs")]
        public IActionResult GetUserVacs(bool arch, Guid userId)
        {
            try
            {
                List<VacancyModel> vacs = (_storage.GetByCondition(vac => vac.UserId == userId && vac.IsArchived == arch)).ToList();

                return Ok(vacs);
            }
            catch
            {
                return new BadRequestResult();
            }
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] VacancyRequestModel vac)
        {
            try
            {
                var newVac = new VacancyModel()
                {
                    Id = Guid.NewGuid(),
                    Email = vac.Email,
                    Name = vac.Name,
                    Location = vac.Location,
                    Occupation = vac.Occupation,
                    Salary = vac.Salary,
                    Description = vac.Description,
                    Requirements = vac.Requirements,
                    UserId = vac.UserId,
                    IsArchived = vac.IsArchived,
                    Firm = vac.Firm
                };
                _storage.Create(newVac);
                return Ok();
            }
            catch(Exception)
            {
                return BadRequest(400);
            }


        }
        

        // PUT api/values/5
        [HttpPut]
        public IActionResult Put([FromBody] VacancyModel vac)
        {
            var oldVac = _storage.GetByCondition(u => u.Id == vac.Id).FirstOrDefault();
            _storage.Delete(oldVac);
            var newVac = new VacancyModel
            {
                Id = vac.Id,
                Email = vac.Email,
                Name = vac.Name,
                Location = vac.Location,
                Occupation = vac.Occupation,
                Salary = vac.Salary,
                Description = vac.Description,
                Requirements = vac.Requirements,
                UserId = vac.UserId,
                IsArchived = vac.IsArchived,
                Firm = vac.Firm
            };
            _storage.Create(newVac);
            return Ok();
        }
        
        // PUT api/values/5
        [Route("Archive")]
        [HttpPut]
        public IActionResult Archive([FromBody]BaseModel id)
        {
            var vac = _storage.GetByCondition(cv => cv.Id == id.Id).FirstOrDefault();
            var newVac = new VacancyModel
                {
                    Id = Guid.NewGuid(),
                    Email = vac.Email,
                    Name = vac.Name,
                    Location = vac.Location,
                    Occupation = vac.Occupation,
                    Salary = vac.Salary,
                    Description = vac.Description,
                    Requirements = vac.Requirements,
                    UserId = vac.UserId,
                    IsArchived = !vac.IsArchived,
                    Firm = vac.Firm
                };
                _storage.Delete(vac);
                _storage.Create(newVac);
                return Ok(200);
            
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var vac = GetVacancyById(id);
            if (vac is null)
            {
                return NotFound();
            }

            _storage.Delete(vac);
            return Ok();

        }

    }
}

