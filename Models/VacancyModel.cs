using System;
using System.Collections.Generic;

namespace JobMarket.Models
{
	public class VacancyModel: BaseModel
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public string Firm { get; set; }
		public string Occupation { get; set; }
		public int Salary { get; set; }
		public string Location { get; set; }
		public string Description { get; set; }
		public string  Requirements { get; set; }
		public bool IsArchived { get; set; }
		public Guid UserId { get; set; }
    }
}

