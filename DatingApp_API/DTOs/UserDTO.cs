using System.ComponentModel.DataAnnotations;

namespace DatingApp_API.DTOs
{
    public class UserDTO
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        [StringLength(12, MinimumLength=4, ErrorMessage="You must specify a Password between 4-12")]
        public string Password { get; set; }
    }
}