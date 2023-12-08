using webNET_Hits_backend_aspnet_project_2.Models.EnumModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webNET_Hits_backend_aspnet_project_2.Models.DbModels
{

    public class UserData
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }

        [Column("Gender", TypeName = "text")]
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
