using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Exam.Models;
using Microsoft.EntityFrameworkCore;

namespace Exam.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<AnActivity> Activities { get; set; }
        public DbSet<ActivityParty> Parties { get; set; }

    }
}