namespace StudentSystem.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Course;
    using StudentSystem.Services.Review;
    using StudentSystem.Services.Review.Models;
    using StudentSystem.ViewModels.Review;
    using StudentSystem.Web.Infrastructure.Extensions;

    using static StudentSystem.Web.Common.NotificationsConstants;

    public class ReviewsController : Controller
    {
        private readonly IReviewService reviewService;
        private readonly ICourseService courseService;
        private readonly UserManager<ApplicationUser> userManager;

        public ReviewsController(
            IReviewService reviewService, 
            ICourseService courseService,
            UserManager<ApplicationUser> userManager)
        {
            this.reviewService = reviewService;
            this.courseService = courseService;
            this.userManager = userManager;
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateReviewBindingModel review)
        {
            var isCourseExist = await this.courseService.IsExistAsync(review.CourseId);
            if (!isCourseExist)
            {
                this.TempData[ERROR_NOTIFICATION] = INVALID_COURSE_MESSAGE;
                return this.RedirectToAction("Index", "Courses");
            }

            var userId = this.User.GetId();
            var isUserInCouse = this.User.IsUserInCourse(this.userManager, review.CourseId, userId);

            if (!isUserInCouse)
            {
                this.TempData[ERROR_NOTIFICATION] = NOT_ALLOWED_TO_ADD_A_REVIEW_MESSAGE;
                return this.RedirectToAction("Index", "Courses");
            }

            var reviewToCreate = new ReviewFormServiceModel
            {
                CourseId = review.CourseId,
                UserId = userId,
                Content = review.Content
            };

            await this.reviewService.CreateAsync(reviewToCreate);

            return this.Ok(review);
        }
    }
}
