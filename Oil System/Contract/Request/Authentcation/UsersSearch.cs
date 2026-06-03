using Oil_System.Contract.Pagination;
using Oil_System.Models;
using System.Text.RegularExpressions;

namespace Oil_System.Contract.Request.Authentcation
{
    public class UsersSearch : PagedRequest<AppUser>
    {
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsActived { get; set; }

        public void AddUsersFilter()
        {
            if (!string.IsNullOrEmpty(Email))
            {
                FilterBy.Add(user => user.Email.Equals(Email));
            }
            if (PhoneNumber != null && Regex.IsMatch(PhoneNumber, "^(?:\\+201|01)[0-2,5][0-9]{8}$\r\n"))
            {
                FilterBy.Add(user => user.PhoneNumber == PhoneNumber);
            }
            FilterBy.Add(user => user.IsActive == IsActived);
        }
    }
}
