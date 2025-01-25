using System.ComponentModel.DataAnnotations;
namespace Carteiras_Digitais.Shared.Dtos
{
    public class UserDto
    {
        [Required(ErrorMessage = "O campo 'Name' é obrigatório.")]
        public string Name { get; set; } = "O campo 'Name' é obrigatório.";

        [Required(ErrorMessage = "O campo 'Email' é obrigatório.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Password' é obrigatório.")]
        public string Password { get; set; } = string.Empty;
    }
}
