using Afsw.Types;
using LiteDB;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace Afsw.Command.IdentityProvider
{
    public class LiteDbUsersTable
    {
        private readonly LiteDatabase _database;
        public LiteDbUsersTable(LiteDatabase database)
        {
            _database = database;
        }

        public IdentityResult Create(ApplicationUserStoreEntry user)
        {
            var users = GetUserCollection();

            if (users.Insert(user) != null)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(new IdentityError { Description = $"Could not insert user {user.Email}." });
        }

        public IdentityResult Update(ApplicationUserStoreEntry user)
        {
            var users = GetUserCollection();

            var storedUser = users.FindOne(u => u.Id == user.Id);

            storedUser.UserName = user.UserName;
            storedUser.Email = user.Email;
            storedUser.EmailConfirmed = user.EmailConfirmed;
            storedUser.PasswordHash = user.PasswordHash;
            storedUser.NormalizedUserName = user.NormalizedUserName;
            storedUser.AuthenticationType = user.AuthenticationType;
            storedUser.IsAuthenticated = user.IsAuthenticated;
            storedUser.Name = user.Name;

            if (users.Update(user))
            {
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError { Description = $"Could not update user {user.Email}." });
        }

        public IdentityResult Delete(Guid userId)
        {
            var users = GetUserCollection();

            if (users.Delete(u => u.Id == userId) > 0)
            {
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError { Description = $"Could not delete user {userId}." });
        }

        public ApplicationUserStoreEntry FindById(Guid userId)
        {
            var users = GetUserCollection();

            return users.Find(u => u.Id == userId).SingleOrDefault();
        }

        public ApplicationUserStoreEntry FindByName(string userName)
        {
            var users = GetUserCollection();

            return users.Find(u => u.NormalizedUserName == userName).SingleOrDefault();
        }

        private LiteCollection<ApplicationUserStoreEntry> GetUserCollection()
            => _database.GetCollection<ApplicationUserStoreEntry>("users");
    }
}
