using System.ComponentModel.DataAnnotations;

namespace JobMarket.Models
{
    public class EmailModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}