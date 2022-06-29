using System;
using System.Collections.Generic;
using System.Linq;
using JobMarket.Files.Workers;
using JobMarket.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobMarket.Controllers
{
    [Route("api/[controller]")]
    public class VacancyController : Controller
    {
        // GET: /<controller>/

        private readonly IGenericCollection<VacancyModel> _collection;
        public VacancyController(IGenericCollection<VacancyModel> collection)
        {
            _collection = collection;
            _collection.StoragePath = "./Files/Settings/VacancyDirectory.json";
            _collection.ReadFromFile();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult GetVacancyById(Guid id)
        {

            var vac =  _collection.GetByCondition(vac => vac.Id == id).FirstOrDefault();
            return Ok(vac);
        }
        
        [HttpGet]
        public IActionResult GetVacs()
        {
            return Ok(_collection.Collection);
        }

        // GET: api/values
        [HttpPost]
        [Route("FilterVacs")]
        public IActionResult GetVacs([FromBody]FilterModel filter)
        {
            try
            {
                var vacs = _collection.GetByCondition(vac =>
                    (string.IsNullOrEmpty(filter.Occupation) || vac.Occupation == filter.Occupation)
                    && (string.IsNullOrEmpty(filter.Name) || vac.Name == filter.Name)
                    && (string.IsNullOrEmpty(filter.Location) || vac.Location == filter.Location)
                    && (!filter.MinSalary.HasValue || vac.Salary >= filter.MinSalary)
                    && (!filter.MaxSalary.HasValue || vac.Salary <= filter.MinSalary)
                    && (vac.IsArchived == false)
                ).ToList();
                return Ok(vacs);
            }
            catch
            {
                return BadRequest();
            }
            
        }
        
        [HttpGet]
        [Route("UserVacs")]
        public IActionResult GetUserVacs(bool arch, Guid userId)
        {
            try
            {
                List<VacancyModel> vacs = (_collection
                                .GetByCondition(vac => vac.UserId == userId && vac.IsArchived == arch))
                                .ToList();
                return Ok(vacs);
            }
            catch
            {
                return BadRequest();
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
                _collection.Create(newVac);
                _collection.Upload();
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
            var oldVac = _collection.GetByCondition(u => u.Id == vac.Id).FirstOrDefault();
            _collection.Delete(oldVac);
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
            _collection.Create(newVac);
            _collection.Upload();
            return Ok();
        }
        
        // PUT api/values/5
        [Route("Archive")]
        [HttpPut]
        public IActionResult Archive([FromBody]BaseModel id)
        {
            var vac = _collection.GetByCondition(cv => cv.Id == id.Id).FirstOrDefault();
            if (vac is not null)
            {
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
                    IsArchived = !vac.IsArchived,
                    Firm = vac.Firm
                };
                
                _collection.Update(newVac);
                _collection.Upload();
                return Ok(200); 
            }
            return BadRequest();


        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete([FromBody]BaseModel id)
        {
            var vac = _collection.GetByCondition(v => v.Id == id.Id ).FirstOrDefault();
            if (vac is null)
            {
                return NotFound();
            }

            _collection.Delete(vac);
            _collection.Upload();
            return Ok();

        }

    }
}

