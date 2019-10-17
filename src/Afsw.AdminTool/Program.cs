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
            using var db = new LiteDatabase(@"C:\Code\Afsw\src\Afsw.Command\store.db");
            var users = db.GetCollection<ApplicationUserStoreEntry>("users");
            
            foreach (var u in users.FindAll())
            {
                Console.WriteLine(u.NormalizedUserName);
            }
        }
    }
}
