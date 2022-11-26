namespace StudentSystem.Services.Administrator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.ViewModels.User;
    using StudentSystem.Web.Data;

    using static StudentSystem.Web.Common.GlobalConstants;

    public class AdministratorService : IAdministratorService
    {
        private readonly StudentSystemDbContext dbContext;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministratorService(
            StudentSystemDbContext dbContext,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUsersAsync()
            => await this.dbContext
                .Users
                .ProjectTo<UserViewModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<bool> PromoteAsync(ApplicationUser user)
        {
            var resultAsTuple = await this.DeterminateNewRole(user);

            var oldRole = resultAsTuple.oldRole;
            var newRole = resultAsTuple.newRole;

            var isPromoted = await this.SetNewRoleAsync(user, oldRole, newRole);

            return isPromoted;
        }

        public async Task<bool> DemoteAsync(ApplicationUser user)
        {
            var resultAsTuple = await this.DeterminateLowerRole(user);

            var oldRole = resultAsTuple.oldRole;
            var newRole = resultAsTuple.newRole;

            var isDemoted = await this.SetNewRoleAsync(user, oldRole, newRole);

            return isDemoted;
        }

        public async Task<bool> BanAsync(string userId)
        {
            if (await this.IsUserBannedAsync(userId))
            {
                return false;
            }

            var user = await this.dbContext.Users.FindAsync(userId);

            user.IsDeleted = true;
            user.DeletedOn = DateTime.UtcNow;

            this.dbContext.Update(user);
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UnbanAsync(string userId)
        {
            if (!await this.IsUserBannedAsync(userId))
            {
                return false;
            }

            var user = await this.dbContext.Users.FindAsync(userId);

            user.IsDeleted = false;
            user.DeletedOn = null;

            this.dbContext.Update(user);
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsUserBannedAsync(string userId)
            => await this
                .dbContext
                .Users
                .AnyAsync(u => u.Id == userId && u.IsDeleted);

        public async Task<bool> IsUserExistAsync(string id)
            => await this
                .dbContext
                .Users
                .AnyAsync(u => u.Id == id);

        private async Task<bool> SetNewRoleAsync(ApplicationUser user, string oldRoleName, string newRoleName)
        {
            var isEmpty = string.IsNullOrEmpty(oldRoleName) || 
                          string.IsNullOrEmpty(newRoleName);
            if (isEmpty)
            {
                return false;
            }

            var oldRole = await this.dbContext
                    .Roles
                    .FirstOrDefaultAsync(r => r.Name == oldRoleName);

            var newRole = await this.dbContext
                    .Roles
                    .FirstOrDefaultAsync(r => r.Name == newRoleName);

            if (oldRole == null || newRole == null)
            {
                throw new ArgumentNullException();
            }

            var oldUserRoleRelation = await this.dbContext
                    .UserRoles
                    .FirstOrDefaultAsync(ur => ur.User == user && ur.Role == oldRole);

            var newUserRoleRelation = new ApplicationUserRole() { User = user, Role = newRole };

            this.dbContext.UserRoles.Remove(oldUserRoleRelation);
            await this.dbContext.UserRoles.AddAsync(newUserRoleRelation);

            await this.dbContext.SaveChangesAsync();

            return true;
        }

        private async Task<(string oldRole, string newRole)> DeterminateNewRole(ApplicationUser user)
        {
            var usersRoles = await this.userManager.GetRolesAsync(user);

            var oldRole = string.Empty;
            var newRole = string.Empty;

            if (usersRoles.Contains(USER_ROLE))
            {
                oldRole = USER_ROLE;
                newRole = STUDENT_ROLE;
            }
            else if (usersRoles.Contains(STUDENT_ROLE))
            {
                oldRole = STUDENT_ROLE;
                newRole = MODERATOR_ROLE;
            }

            return (oldRole, newRole);
        }

        private async Task<(string oldRole, string newRole)> DeterminateLowerRole(ApplicationUser user)
        {
            var usersRoles = await this.userManager.GetRolesAsync(user);

            var oldRole = string.Empty;
            var newRole = string.Empty;

            if (usersRoles.Contains(STUDENT_ROLE))
            {
                oldRole = STUDENT_ROLE;
                newRole = USER_ROLE;
            }
            else if (usersRoles.Contains(MODERATOR_ROLE))
            {
                oldRole = MODERATOR_ROLE;
                newRole = STUDENT_ROLE;
            }

            return (oldRole, newRole);
        }
    }
}