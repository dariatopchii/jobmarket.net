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
        public CvModel GetCvById(int id)
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

        // POST api/values
        [HttpPost]
        public void Post([FromBody] CvModel cv)
        {
            var vac = _storage.GetByCondition(u => u.Id == cv.Id).FirstOrDefault();

            if (vac is not null)
            {
                throw new Exception("cv with id {id} already exists");
            }
            _storage.Create(cv);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] CvModel cv)
        {
            var oldCv = _storage.GetByCondition(u => u.Id == id).FirstOrDefault();
            _storage.Delete(oldCv);
            _storage.Create(cv);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var cv = this.GetCvById(id);
            if (cv is null)
            {
                throw new Exception("CV with id {id} does not exist");
            }

            _storage.Delete(cv);
        }

    }
}

