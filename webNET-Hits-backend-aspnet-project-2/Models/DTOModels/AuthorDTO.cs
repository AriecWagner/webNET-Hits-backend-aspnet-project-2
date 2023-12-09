using webNET_Hits_backend_aspnet_project_2.Models.EnumModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_2.Models.AnotherModels
{
    public class AuthorDTO
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        [Column("Gender", TypeName = "text")]
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }
        public int Posts { get; set; }
        public int Likes { get; set; }
        public DateTime Created { get; set; }
    }
}
