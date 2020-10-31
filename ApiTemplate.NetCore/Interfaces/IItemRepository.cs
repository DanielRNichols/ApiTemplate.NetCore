using ApiTemplate.NetCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTemplate.NetCore.Interfaces
{
    public interface IItemRepository : IBaseRepository<Item>
    {
    }
}
