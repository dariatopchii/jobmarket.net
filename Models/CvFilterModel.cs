using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobMarket.Models
{
	public class CvFilterModel
	{
		public string Email { get; set; }
		public string Name { get; set; }
		public string Location { get; set; }
		public string Occupation { get; set; }
		public int Salary { get; set; }

	}
}

