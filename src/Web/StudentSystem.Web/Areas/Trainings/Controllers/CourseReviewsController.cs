namespace StudentSystem.Web.Areas.Trainings.Courses.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Course;
    using StudentSystem.Services.Review;
    using StudentSystem.Services.Review.Models;
    using StudentSystem.Web.Areas.Trainings.Controllers.Abstraction;
    using StudentSystem.Web.Infrastructure.Extensions;

    using static StudentSystem.Web.Common.NotificationsConstants;

    public class CourseReviewsController : TrainingController
    {
        private const string DELETE = "delete";
        private const string UPDATE = "update";

        private readonly IReviewService reviewService;
        private readonly ICourseService courseService;
        private readonly UserManager<ApplicationUser> userManager;

        public CourseReviewsController(
            IReviewService reviewService,
            ICourseService courseService,
            UserManager<ApplicationUser> userManager)
        {
            this.reviewService = reviewService;
            this.courseService = courseService;
            this.userManager = userManager;
        }


        [HttpPost]
        public async Task<IActionResult> Create(int courseId, ReviewFormServiceModel review)
        {
            var courseIdAsAnonymousObj = new { Id = courseId };
            var isCourseExist = await courseService.IsExistAsync(courseId);

            if (!isCourseExist)
            {
                TempData[ERROR_NOTIFICATION] = INVALID_COURSE_MESSAGE;

                return RedirectToAction("Index", "Courses");
            }

            var userId = User.GetId();
            var isUserInCouse = User.IsUserInCourse(userManager, courseId, userId);

            if (!isUserInCouse)
            {
                TempData[ERROR_NOTIFICATION] = NOT_ALLOWED_TO_ADD_A_REVIEW_MESSAGE;

                return RedirectToAction("Details", "Courses", courseIdAsAnonymousObj);
            }

            var reviewToCreate = new CreateReviewServiceModel
            {
                CourseId = courseId,
                UserId = userId,
                Content = review.Content
            };

            await reviewService.CreateEntityAsync(reviewToCreate);

            return RedirectToAction("Details", "Courses", courseIdAsAnonymousObj);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var review = await reviewService.GetByIdAsync<ReviewFormServiceModel>(id);

            if (review == null)
            {
                TempData[ERROR_NOTIFICATION] = INVALID_REVIEW_MESSAGE;

                return RedirectToAction("Index", "Home");
            }

            return View(review);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, int courseId, ReviewFormServiceModel review)
        {
            if (!ModelState.IsValid)
            {
                return View(review);
            }

            var isValid = await IsAuthorOrAdminAsync(id);
            if (!isValid)
            {
                TempData[ERROR_NOTIFICATION] = string.Format(NOT_HAVE_PERMISSION_MESSAGE, UPDATE);

                return RedirectToAction("Index", "Home");
            }

            await reviewService.UpdateEntityAsync(id, review);

            return RedirectToAction("Details", "Courses", new { Id = courseId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int reviewId, int courseId)
        {
            var review = await reviewService.GetByIdAsync<ReviewUserIdServiceModel>(reviewId);

            if (review == null)
            {
                TempData[ERROR_NOTIFICATION] = INVALID_REVIEW_MESSAGE;

                return RedirectToAction("Index", "Home");
            }

            var isValid = await IsAuthorOrAdminAsync(reviewId);

            if (!isValid)
            {
                TempData[ERROR_NOTIFICATION] = string.Format(NOT_HAVE_PERMISSION_MESSAGE, DELETE);

                return RedirectToAction("Index", "Home");
            }

            await reviewService.DeleteAsync(reviewId);

            return RedirectToAction("Details", "Courses", new { Id = courseId });
        }

        private async Task<bool> IsAuthorOrAdminAsync(int reviewId)
        {
            var review = await reviewService.GetByIdAsync<ReviewUserIdServiceModel>(reviewId);
            var isAuthorOrAdmin = review.UserId == User.GetId() || User.IsAdministrator();

            return isAuthorOrAdmin;
        }
    }
}
