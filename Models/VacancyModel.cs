using System;
using System.Collections.Generic;

namespace JobMarket.Models
{
	public class VacancyModel: BaseModel
	{
		public string Firm { get; set; }
		public string Position { get; set; }
		public int Salary { get; set; }
		public string Workplace { get; set; }
		public string Description { get; set; }
		public string  Requirements { get; set; }

		public static explicit operator VacancyModel(Func<IEnumerable<VacancyModel>> v)
        {
            throw new NotImplementedException();
        }
    }
}

