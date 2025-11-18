using System.ComponentModel.DataAnnotations;

namespace MAlex.Models.viewModels
{
    public class RoleViewModel
    {
        [Display(Name ="Role Name")]
        public string RoleName { get; set; }

    }
}
