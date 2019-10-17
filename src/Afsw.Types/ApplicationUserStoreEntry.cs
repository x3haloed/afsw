using System;

namespace Afsw.Types
{
    public class ApplicationUserStoreEntry
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string NormalizedUserName { get; set; }
        public string AuthenticationType { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Name { get; set; }
        public string[] Roles { get; set; } = Array.Empty<string>();
    }
}
