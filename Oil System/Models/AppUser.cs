using Microsoft.AspNetCore.Identity;

namespace Oil_System.Models
{
    public class AppUser : IdentityUser
    {
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; }
    }
}
