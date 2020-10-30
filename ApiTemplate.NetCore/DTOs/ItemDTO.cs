using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTemplate.NetCore.DTOs
{
    public class ItemDTO
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; }
        public string Properties { get; set; }
    }
}
