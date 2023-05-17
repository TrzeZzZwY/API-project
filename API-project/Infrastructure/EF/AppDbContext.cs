using AppCore.Models;
using Infrastructure.EF.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF
{
    public class AppDbContext : IdentityDbContext<User, UserRole, Guid>
    {
    }
}
