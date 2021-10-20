using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";
    }
}
