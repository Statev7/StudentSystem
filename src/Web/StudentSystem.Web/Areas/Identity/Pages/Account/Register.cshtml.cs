namespace StudentSystem.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.City;
    using StudentSystem.ViewModels.City;
    using StudentSystem.Web.Common;

    using static StudentSystem.Data.Common.Constants;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private const string CITIES_KEY = "Cities";

        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICityService cityService;
        private readonly IMemoryCache memoryCache;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ICityService cityService,
            IMemoryCache memoryCache)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.cityService = cityService;
            this.memoryCache = memoryCache;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        //TODO: Move error messages to contants

        public class InputModel
        {
            [Required]
            [MaxLength(FIRST_NAME_MAX_LENGTH)]
            [MinLength(FIRST_NAME_MIN_LENGTH)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [MaxLength(LAST_NAME_MAX_LENGTH)]
            [MinLength(LAST_NAME_MIN_LENGTH)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(PASSWORD_MAX_LENGTH, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = PASSWORD_MIN_LENGTH)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public int? CityId { get; set; }

            public ICollection<CityIdNameViewModel> Cities { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.Redirect("/");
            }

            ReturnUrl = returnUrl;
            ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            var cities = (List<CityIdNameViewModel>)this.memoryCache.Get(CITIES_KEY);

            if (cities == null)
            {
                cities = await this.cityService
                        .GetAllAsQueryable<CityIdNameViewModel>()
                        .OrderBy(x => x.Name)
                        .ToListAsync();

                this.memoryCache.Set(CITIES_KEY, cities, TimeSpan.FromDays(1)); 
            }

            this.ViewData["Cities"] = cities;

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.Redirect("/");
            }

            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                if (Input.CityId.Value == 0)
                {
                    Input.CityId = null;
                }

                var user = new ApplicationUser 
                { 
                    FirstName = Input.FirstName,
                    LastName = Input.LastName, 
                    UserName = Input.Email, 
                    Email = Input.Email,
                    CreatedOn = DateTime.UtcNow,
                    CityId = Input.CityId
                };

                var result = await this.userManager.CreateAsync(user, Input.Password);
                await this.userManager.AddToRoleAsync(user, GlobalConstants.USER_ROLE);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }
    }
}
