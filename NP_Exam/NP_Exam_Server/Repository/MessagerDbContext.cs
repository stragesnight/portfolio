using System;
using System.Data.Entity;
using NP_Exam_Server.Model;

namespace NP_Exam_Server.Repository
{
    public class MessagerDbContext : DbContext
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UsersGroups { get; set; }

        public MessagerDbContext() : base("MessagerDB")
        {
            Database.SetInitializer(new MessagerDbInitializer());
        }
    }
}
