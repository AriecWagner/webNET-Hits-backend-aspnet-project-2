﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webNET_Hits_backend_aspnet_project_2.Models.DbModels
{
    [Table("as_houses", Schema = "fias")]
    public class AsHouses
    {
        [Key]
        public long id { get; set; }
        public long objectid { get; set; }
        public Guid objectguid { get; set; }
        public long changeid { get; set; }
        public string housenum { get; set; }
        public string? addnum1 { get; set; }
        public string? addnum2 { get; set; }
        public int housetype { get; set; }
        public int? addtype1 { get; set; }
        public int? addtype2 { get; set; }
        public int opertypeid { get; set; }
        public long previd { get; set; }
        public long nextid { get; set; }
        public DateTime updatedate { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public int isactual { get; set; }
        public int isactive { get; set; }
    }
}
