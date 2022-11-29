namespace StudentSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
	using System.Security.Claims;
	using System.Threading.Tasks;

	using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.City;
	using StudentSystem.Services.User;
	using StudentSystem.Services.User.Models;
	using StudentSystem.ViewModels.City;

    using static StudentSystem.Web.Common.GlobalConstants;

    public class UserController : Controller
	{
		private readonly IUserService userService;
		private readonly ICityService cityService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

		public UserController(
            IUserService userService, 
            ICityService cityService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
		{
			this.userService = userService;
			this.cityService = cityService;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginServiceModel loginModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(loginModel);
            }

            var user = await this.userManager.FindByEmailAsync(loginModel.Email);
            if (user != null)
            {
                if (user.IsDeleted)
                {
                    ModelState.AddModelError(string.Empty, "You are banned!");
                    return this.View(loginModel);
                }

                var result = await this.signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);
                if (!result.Succeeded)
                {
                    this.ModelState.AddModelError("", "Invalid user");
                    return this.View(loginModel);
                }
            }

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }

            var registerModel = new CreateUserServiceModel()
            {
                Cities = await this.GetCitiesOrderedByNameAsync(),
            };

            return this.View(registerModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(CreateUserServiceModel registerModel)
        {
            if (!this.ModelState.IsValid)
            {
                registerModel.Cities = await this.GetCitiesOrderedByNameAsync();
                return this.View(registerModel);
            }

            var user = new ApplicationUser()
            {
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                Email = registerModel.Email,
                UserName = registerModel.Email,
                ImageURL = registerModel.ImageUrl,
                CityId = registerModel.CityId ?? null,
                CreatedOn = DateTime.UtcNow,
            };

            var result = await this.userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded)
            {
                await this.userManager.AddToRoleAsync(user, USER_ROLE);
                await this.signInManager.SignInAsync(user, false);

                return this.RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError("", error.Description);
            }

            registerModel.Cities = await this.GetCitiesOrderedByNameAsync();
            return this.View(registerModel);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();

            return this.RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Account()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await this.userService.GetByIdAsync<UserDetailsServiceModel>(userId);

            return this.View(user);
        }

        [Authorize]
        [HttpGet]
		public async Task<IActionResult> Edit()
		{
			var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await this.userService.GetByIdAsync<CreateUserServiceModel>(userId);

            user.Cities = await this.GetCitiesOrderedByNameAsync();

            return this.View(user);
		}

        [Authorize]
        [HttpPost]
		public async Task<IActionResult> Edit(CreateUserServiceModel user)
		{
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

			await this.userService.UpdateAsync(userId, user);

            return this.RedirectToAction("Index", "Home");
		}

        private async Task<IEnumerable<CityIdNameViewModel>> GetCitiesOrderedByNameAsync()
            => await this.cityService
                .GetAllAsQueryable<CityIdNameViewModel>()
                .OrderBy(c => c.Name)
                .ToListAsync();
    }
}
