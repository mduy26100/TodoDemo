using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.ApplicationIdentity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string? Description { get; set; }

        public ApplicationRole() : base()
        {
            
        }

        public ApplicationRole(string roleName, string description) : base(roleName)
        {
            Description = description;
        }
    }
}
