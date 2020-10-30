using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTemplate.NetCore.Data
{
    [Table("Items")]
    public partial class Item
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; }
        public string Properties { get; set; }
    }
}
