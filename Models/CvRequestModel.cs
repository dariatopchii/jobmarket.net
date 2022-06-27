using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobMarket.Models
{
	public class CvRequestModel
	{
		public string Email { get; set; }
		public string Name { get; set; }
		public string Location { get; set; }
		public string Occupation { get; set; }
		public string Education { get; set; }
		public string Workplace { get; set; }
		public string Firm { get; set; }
		public string Position { get; set; }
		public int Salary { get; set; }
		public string Description { get; set; }
		public string  Requirements { get; set; }
		public Guid UserId { get; set; }
		public bool IsArchived { get; set; }
		
	}
}

