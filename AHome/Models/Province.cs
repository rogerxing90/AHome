using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AHome.Models
{
    public class Province
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]//主键不自增
        public System.Int32 Id { get; set; }
        public System.Int32 ParentId { get; set; }
        public System.String CityName { get; set; }
    }
}