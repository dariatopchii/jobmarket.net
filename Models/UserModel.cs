﻿using System;
using System.Collections.Generic;

namespace JobMarket.Models
{
	public class UserModel : BaseModel
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string Name { get; set; }
		
		public List<Guid> CvId { get; set; }
	}
}

