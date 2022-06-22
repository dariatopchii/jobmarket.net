﻿using System.ComponentModel.DataAnnotations;

namespace JobMarket.Models
{
	public class UserRegistrationModel : BaseModel
	{
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

