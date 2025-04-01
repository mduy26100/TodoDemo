using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.ApplicationIdentity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FullName { get; set; }

        //Token
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExprytime { get; set; }
    }
}
