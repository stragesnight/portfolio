using System;
using System.Data.Entity;
using System.Collections.Generic;
using NP_Exam_Server.Model;

namespace NP_Exam_Server.Repository
{
    public class MessagerDbInitializer : DropCreateDatabaseAlways<MessagerDbContext>
    {
        protected override void Seed(MessagerDbContext context)
        {
            context.Users.Add(new User("1", "1"));
            context.Users.Add(new User("2", "2"));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
