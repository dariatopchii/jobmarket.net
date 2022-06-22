using System.ComponentModel.DataAnnotations;

namespace JobMarket.Models
{
	public class UserLoginModel : BaseModel
	{
		[Required]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}

