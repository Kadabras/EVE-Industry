using EVE_Industry.EfStuff.DbModel;
using EVE_Industry.EfStuff.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVE_Industry.EfStuff.Repositories
{
    public class MainIndustryRepository : BaseRepository<MainIndustryCell>
    {
        public MainIndustryRepository(WebContext webContext) : base(webContext)
        {
        }

    }
}
