using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hubtel.Wallets.Api.Models
{
    [Table("Users")]
    public class User
    {
        [Key, Required]
        public int Id { get; set; }

        public string Email { get; set; }  
        public string Name { get; set; }
        public string Password { get; set; }
        public string Token { get; set; } = string.Empty;
        public string PhoneNumber { get; set; }
    }
}
