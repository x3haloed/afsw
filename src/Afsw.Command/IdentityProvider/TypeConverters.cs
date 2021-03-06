﻿using Afsw.Types;

namespace Afsw.Command.IdentityProvider
{
    public static class TypeConverters
    {
        public static ApplicationUserStoreEntry ToStoreEntry(this ApplicationUser user)
        {
            return new ApplicationUserStoreEntry
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PasswordHash = user.PasswordHash,
                NormalizedUserName = user.NormalizedUserName,
                AuthenticationType = user.AuthenticationType,
                IsAuthenticated = user.IsAuthenticated,
                Name = user.Name,
            };
        }

        public static ApplicationUser ToApplicationUser(this ApplicationUserStoreEntry storeEntry)
        {
            return new ApplicationUser
            {
                Id = storeEntry.Id,
                UserName = storeEntry.UserName,
                Email = storeEntry.Email,
                EmailConfirmed = storeEntry.EmailConfirmed,
                PasswordHash = storeEntry.PasswordHash,
                NormalizedUserName = storeEntry.NormalizedUserName,
                AuthenticationType = storeEntry.AuthenticationType,
                IsAuthenticated = storeEntry.IsAuthenticated,
                Name = storeEntry.Name,
            };
        }
    }
}
