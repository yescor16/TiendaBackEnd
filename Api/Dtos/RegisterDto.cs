using System.ComponentModel.DataAnnotations;

namespace Api.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName{ get; set; }
        [Required]
        [EmailAddress]
        public string Email{ get; set; }
        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$",
            ErrorMessage ="Pasword debe tener 1 Mayuscula, 1 Minuscula, 1 numero, 1 no Alfanumerico y minimo 6 caracteres")]
        public string Password { get; set; }
    }
}