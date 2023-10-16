using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EVE_Industry.Models
{
    public class DumpTaskModel
    {
        public long Id { get; set; }
        public int siteId { get; set; }
        public string Name { get; set; }

        public CancellationTokenSource CancellationTokenSource { get; set; }
    }
}
