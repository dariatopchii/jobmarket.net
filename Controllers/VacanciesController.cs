using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JobMarket.Files.Interfaces;
using JobMarket.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JobMarket.Controllers
{
    [Route("api/[controller]")]
    public class VacanciesController : Controller
    {
        private readonly IGenericStorageWorker<VacancyModel> _storage;

        public VacanciesController(IGenericStorageWorker<VacancyModel> storage)
        {
            _storage = storage;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public VacancyModel GetVacancyById(int id)
        {

            return (_storage.GetByCondition(u => u.Id == id)).FirstOrDefault();
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<VacancyModel> Get()
        {
            var vacancies = _storage.GetAll();
            return vacancies;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]VacancyModel vacancy)
        {
            var vac = _storage.GetByCondition(u => u.Id == vacancy.Id).FirstOrDefault();

            if (vac is not null)
            {
                throw new Exception("vacancy with id {id} already exists");
            }
            _storage.Create(vacancy);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]VacancyModel vacancy)
        {
           var oldVacancy = _storage.GetByCondition(u => u.Id == id).FirstOrDefault();
            _storage.Delete(oldVacancy);
            _storage.Create(vacancy);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var vacancy = this.GetVacancyById(id);
            if (vacancy is null)
            {
                throw new Exception("vacancy with id {id} does not exist");
            }

            _storage.Delete(vacancy);
        }
    }
}

