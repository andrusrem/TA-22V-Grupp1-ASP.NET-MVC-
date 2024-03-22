using System;
using System.Threading.Tasks;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace KooliProjekt.IntegrationTests.Helpers
{
    public abstract class TestBase : IDisposable
    {
        public WebApplicationFactory<FakeStartup> Factory { get; private set; }

        public TestBase()
        {
            Factory = new TestApplicationFactory<FakeStartup>();
        }

        protected void AuthenticateAdmin()
        {
            SwitchFactory(TestClaimsProvider.WithAdminClaims());
        }

        protected void AuthenticateUser()
        {
            SwitchFactory(TestClaimsProvider.WithUserClaims());
        }

        protected ApplicationDbContext GetDbContext()
        {
            var dbContext = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));
            dbContext.Database.EnsureCreated();

            return dbContext;
        }

        public async Task AddUser(bool isAdmin = false)
        {
            var userManager = Factory.Services.GetService<UserManager<IdentityUser>>();
            var user = new IdentityUser();
            user.UserName = isAdmin ? "admin@example.com" : "user@example.com";
            user.EmailConfirmed = true;
            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, "Polükas2013*");
            user.Email = user.UserName;
            user.NormalizedEmail = user.Email.ToUpper();
            user.Id = user.UserName;

            await userManager.CreateAsync(user);

            if (isAdmin)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }

        private void SwitchFactory(TestClaimsProvider provider)
        {
            DisposeFactory();

            Factory = Factory.WithAuthentication(provider);
            GetDbContext().Database.EnsureCreated();
        }

        public void Dispose()
        {
            DisposeFactory();
        }

        private void DisposeFactory()
        {
            var dbContext = GetDbContext();
            dbContext.Database.EnsureDeleted();

            Factory.Dispose();
        }

        // Add you other helper methods here
    }
}