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

    using StudentSystem.Data.Models.Abstraction;
    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Abstaction;
    using StudentSystem.ViewModels.User;
    using StudentSystem.Web.Data;

    using static StudentSystem.Web.Common.GlobalConstants;

    public class AdministratorService : BaseService<BaseModel>, IAdministratorService
    {
        private const int USERS_PER_PAGE = 5;

        private readonly UserManager<ApplicationUser> userManager;

        public AdministratorService(
            StudentSystemDbContext dbContext,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)   
            :base(dbContext, mapper)
        {
            this.userManager = userManager;
        }

        public UserPageViewModel GetUsers(string search, int currentPage)
        {
            var query = this.DbContext
                .Users
                .OrderBy(u => u.UserRoles
                    .Select(ur => ur.Role.Name)
                    .FirstOrDefault())
                .ThenBy(u => u.CreatedOn)
                .ProjectTo<UserViewModel>(this.Mapper.ConfigurationProvider)
                .AsQueryable();

            if (search != null)
            {
                search = search.ToLower();

                query = query.Where(u =>
                    u.FirstName.ToLower().Contains(search) ||
                    u.LastName.ToLower().Contains(search) ||
                    u.CityName.ToLower().Contains(search));
            }

            var usersCount = query.Count();
            var users = new List<UserViewModel>();

            if (query.Any())
            {
                users = this
                    .Paging(query, currentPage, USERS_PER_PAGE)
                    .ToList();
            }

            var model = new UserPageViewModel()
            {
                CurrentPage = currentPage,
                TotalEntities = usersCount,
                Search = search == null ? search : search.ToLower(),
                EntitiesPerPage = USERS_PER_PAGE,
                Users = users,
            };

            return model;
        }

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

            var user = await this.DbContext.Users.FindAsync(userId);

            user.IsDeleted = true;
            user.DeletedOn = DateTime.UtcNow;

            this.DbContext.Update(user);
            await this.DbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UnbanAsync(string userId)
        {
            if (!await this.IsUserBannedAsync(userId))
            {
                return false;
            }

            var user = await this.DbContext.Users.FindAsync(userId);

            user.IsDeleted = false;
            user.DeletedOn = null;

            this.DbContext.Update(user);
            await this.DbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsUserBannedAsync(string userId)
            => await this
                .DbContext
                .Users
                .AnyAsync(u => u.Id == userId && u.IsDeleted);

        public async Task<bool> IsUserExistAsync(string id)
            => await this
                .DbContext
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

            await this.userManager.RemoveFromRoleAsync(user, oldRoleName);
            await this.userManager.AddToRoleAsync(user, newRoleName);

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

            return (oldRole, newRole);
        }
    }
}