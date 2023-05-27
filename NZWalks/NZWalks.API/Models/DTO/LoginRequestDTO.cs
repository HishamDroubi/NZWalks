using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class LoginRequestDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public String UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; }
    }
}
