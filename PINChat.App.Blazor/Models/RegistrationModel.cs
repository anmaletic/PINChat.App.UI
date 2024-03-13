using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PINChat.App.Blazor.Models;

public class RegistrationModel
{
    [Required(ErrorMessage = "Korisničko ime mora biti upisano.")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Lozinka mora biti upisana.")]
    [MinLength(6, ErrorMessage = "Lozinka mora sadržavati minimalno 6 znakova.")]
    [CustomPassword(ErrorMessage = "Lozinka mora sadržavati veliko i malo slovo (engleske abecede) te specijalne znakove.")]
    public string? Password { get; set; }
    
    [Compare("Password", ErrorMessage = "Lozinka i potvrda lozinke nisu iste.")]
    public string? ConfirmPassword { get; set; }
    

    public void Reset()
    {
        UserName = null;
        Password = null;
        ConfirmPassword = null;
    }
    
    
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CustomPasswordAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is string password)
            {
                // Define your password requirements using a regular expression.
                // This example requires at least one uppercase letter, one lowercase letter,
                // one special character, and a minimum length of 6 characters.
                string pattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*[\W_])";

                if (Regex.IsMatch(password, pattern))
                {
                    return true;
                }
            }

            return false;
        }
    }
}