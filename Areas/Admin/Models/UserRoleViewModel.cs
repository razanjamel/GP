using Microsoft.AspNetCore.Mvc.Rendering;

namespace GP.Areas.Admin.Models
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<SelectListItem> Roles { get; set; }
        public string[] SelectedRoles { get; set; }
    }
}
