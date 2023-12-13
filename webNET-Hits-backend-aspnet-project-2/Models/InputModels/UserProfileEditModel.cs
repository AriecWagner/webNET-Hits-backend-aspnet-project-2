using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using webNET_Hits_backend_aspnet_project_2.Models.EnumModels;

namespace webNET_Hits_backend_aspnet_project_2.Models.InputModels
{
    public class UserProfileEditModel
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }

        [Column("Gender", TypeName = "text")]
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
    }
}
