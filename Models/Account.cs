using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiddleTestAuthetication.Models
{
    public class Account : IdentityUser
    {
        [DisplayName("Identify Number")]
        public string IdNumber { get; set; }
        [DisplayName("Phone Number")]
        public string Phone { get; set; }
        [DisplayName("Status")]
        [Range(-1,1)]
        public int Status { get; set; }
    }
}