using System.Collections.Generic;
using System.Security.Claims;

namespace KooliProjekt.IntegrationTests.Helpers
{
    public class TestClaimsProvider
    {
        public IList<Claim> Claims { get; }

        public TestClaimsProvider(IList<Claim> claims)
        {
            Claims = claims;
        }

        public TestClaimsProvider()
        {
            Claims = new List<Claim>();
        }

        public static TestClaimsProvider WithAdminClaims()
        {
            var provider = new TestClaimsProvider();
            provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, "admin@example.com"));
            provider.Claims.Add(new Claim(ClaimTypes.Name, "admin@example.com"));
            provider.Claims.Add(new Claim(ClaimTypes.Role, "Admin"));

            return provider;
        }

        public static TestClaimsProvider WithUserClaims()
        {
            var provider = new TestClaimsProvider();
            provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, "user@example.com"));
            provider.Claims.Add(new Claim(ClaimTypes.Name, "user@example.com"));

            return provider;
        }
    }
}