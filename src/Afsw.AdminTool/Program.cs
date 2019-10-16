using Afsw.Types;
using LiteDB;
using System;
using System.Linq;

namespace Afsw.AdminTool
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new LiteDatabase("store.db");
            var users = db.GetCollection<ApplicationUserStoreEntry>("users");
            var chad = users.Find(u => u.Name == "chad").Single();
            if (chad != null)
            {
                users.Delete(chad.Id);
            }
            users.Insert(new ApplicationUserStoreEntry
            {

                UserName = "chad",
                Email = "chad@ch.ad",
                EmailConfirmed = true,
                PasswordHash = "poopoo",
                AuthenticationType = "ummm",
                IsAuthenticated = false,
                Name = "chad",
                Roles = new [] { "8e1e72fb-6a2a-4320-8249-77fc00fa24fa" }
            });
        }
    }
}
