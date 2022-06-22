using System;
using System.Collections.Generic;

namespace JobMarket.Models
{
	public class CvModel: BaseModel
	{
		public string Position { get; set; }
		public int Salary { get; set; }
		public string Location { get; set; }

        public static explicit operator CvModel(Func<IEnumerable<CvModel>> v)
        {
            throw new NotImplementedException();
        }
    }
}

