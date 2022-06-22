using System;
using System.Collections.Generic;

namespace JobMarket.Models
{
	public class VacancyModel: BaseModel
	{
		public string Position { get; set; }
		public int Salary { get; set; }
		public string Location { get; set; }

        public static explicit operator VacancyModel(Func<IEnumerable<VacancyModel>> v)
        {
            throw new NotImplementedException();
        }
    }
}

