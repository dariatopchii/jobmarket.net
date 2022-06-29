using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JobMarket.Files.Workers;
using JobMarket.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobMarket.Controllers
{
    [Route("api/[controller]")]
    public class CvController : Controller
    {
        // GET: /<controller>/
        private readonly IGenericCollection<CvModel> _collection;
        public CvController(IGenericCollection<CvModel> collection)
        {
            _collection = collection;
            _collection.StoragePath = "./Files/Settings/CvDirectory.json";
            _collection.ReadFromFile();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public CvModel GetCvById(Guid id)
        {

            return (_collection.GetByCondition(cv => cv.Id == id)).FirstOrDefault();
            
        }
        
        // GET: api/values
        [HttpGet]
        public IActionResult GetCvs()
        {
            return Ok(_collection.Collection);
        }
        
        [HttpGet]
        [Route("UserCvs")]
        public IActionResult GetUserCvs(bool arch, Guid userId)
        {
            try
            {
                List<CvModel> cvs = _collection.GetByCondition(u => u.UserId == userId && u.IsArchived == arch).ToList();
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
            var cvs = _collection.GetByCondition(cv =>
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
                _collection.Create(newCv);
                return Ok();
            }
            catch
            {
                return BadRequest(400);
            }


        }
        

        // PUT api/values/5
        [HttpPut]
        public void EditCv([FromBody] CvModel cv)
        {
            var oldCv = _collection.GetByCondition(u => u.Id == cv.Id).FirstOrDefault();
            _collection.Delete(oldCv);
            var newCv = new CvModel
            {
                Id = cv.Id,
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
            _collection.Create(newCv);
        }
        
        // PUT api/values/5
        [Route("Archive")]
        [HttpPut]
        public IActionResult Archive([FromBody]BaseModel id)
        {
            var cv = _collection.GetByCondition(cv => cv.Id == id.Id).FirstOrDefault();
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
                    IsArchived = !cv.IsArchived
                };
                _collection.Delete(cv);
                _collection.Create(newCv);
                return Ok(200);
            }
            catch
            {
                return BadRequest(400);
            }

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

            _collection.Delete(cv);
            return Ok();

        }
        
        [HttpGet("{id}/file")]
        public ActionResult<Stream> DownloadFile(Guid id)
        {
            var cv = (_collection.GetByCondition(cv => cv.Id == id)).FirstOrDefault();
            if (cv is null)
            {
                return NotFound();
            }
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            var serviceProperties = new[]
            {
                nameof(CvModel.Id),
                nameof(CvModel.UserId),
                nameof(CvModel.IsArchived),
            };
            var properties = cv.GetType().GetProperties()
                .Where(prop => !serviceProperties.Contains(prop.Name));
            foreach (var prop in properties)
            {
                writer.WriteLine($"{prop.Name}: {prop.GetValue(cv)}");
            }
            
            writer.Flush();
            ms.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(ms, "application/octet-stream");
        }

    }
}

