using Microsoft.AspNet.Identity.EntityFramework;
using MiddleTestAuthetication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiddleTestAuthetication.Data
{
    public class MyIdentityDbContext : IdentityDbContext<Account>
    {
        public MyIdentityDbContext() : base("ConnectionString")
        {

        }
    }
}