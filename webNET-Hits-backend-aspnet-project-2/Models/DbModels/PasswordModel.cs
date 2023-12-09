using System.ComponentModel.DataAnnotations;

namespace webNET_Hits_backend_aspnet_project_2.Models.DbModels
{
    public class PasswordModel
    {
        [Key]
        public Guid UserId { get; set; }
        public string HashedPassword { get; set; }
    }
}
