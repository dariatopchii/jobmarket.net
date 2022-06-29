using System;

namespace JobMarket.Models
{
	public class VacancyRequestModel
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public string Firm { get; set; }
		public string Occupation { get; set; }
		public int Salary { get; set; }
		public string Location { get; set; }
		public string Description { get; set; }
		public bool IsArchived { get; set; }
		public Guid UserId { get; set; }
    }
}

