using System.ComponentModel.DataAnnotations;
using webNET_Hits_backend_aspnet_project_2.Models.EnumModels;

namespace webNET_Hits_backend_aspnet_project_2.Models.InputModels
{
    public class InputUserRegisterModel
    {
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
    }
}
