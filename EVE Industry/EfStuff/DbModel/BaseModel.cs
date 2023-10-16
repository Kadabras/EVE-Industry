using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVE_Industry.EfStuff.DbModel
{
    public abstract class BaseModel
    {
        public long Id { get; set; }
        public int TypeId { get; set; }
        public int ParsedId { get; set; }

    }
}
