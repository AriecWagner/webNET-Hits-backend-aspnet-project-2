using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webNET_Hits_backend_aspnet_project_2.Models.DbModels
{
    [Table("as_adm_hierarchy", Schema = "fias")]
    public class AsAdmHierarchy
    {
        [Key]
        public long id { get; set; }
        public long objectid { get; set; }
        public long parentobjid { get; set; }
        public long changeid { get; set; }
        public string regioncode { get; set; }
        public string? areacode { get; set; }
        public string? citycode { get; set; }
        public string? placecode { get; set; }
        public string? plancode { get; set; }
        public string? streetcode { get; set; }
        public long previd { get; set; }
        public long nextid { get; set; }
        public DateTime updatedate { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public int isactive { get; set; }
        public string path { get; set; }
    }
}
