namespace StudentSystem.Web.Areas.Trainings.Courses.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Data.Models.StudentSystem;
    using StudentSystem.Services.Course;
    using StudentSystem.Services.Review;
    using StudentSystem.Services.Review.Models;
    using StudentSystem.Web.Areas.Trainings.Controllers.Abstraction;
    using StudentSystem.Web.Infrastructure.Extensions;
    using StudentSystem.Web.Infrastructure.Filters;

    using static StudentSystem.Web.Common.NotificationsConstants;

    [Authorize]
    [TypeFilter(typeof(UserStatusFilter))]
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
        public async Task<IActionResult> Create(int courseId, string content)
        {
            //if (!this.ModelState.IsValid)
            //{
            //   return this.View(review);
            //}

            var courseIdAsObject = new { Id = courseId };

            var isCourseExist = await this.courseService.IsExistAsync(courseId);
            if (!isCourseExist)
            {
                this.TempData[ERROR_NOTIFICATION] =
                    string.Format(SUCH_A_ENTITY_DOES_NOT_EXIST, COURSE_KEYWORD);

                return this.RedirectToAction("Index", "Courses");
            }

            var userId = this.User.GetId();
            var isUserInCouse = this.User.IsUserInCourse(this.userManager, courseId, userId);

            if (!isUserInCouse)
            {
                this.TempData[ERROR_NOTIFICATION] = NOT_ALLOWED_TO_ADD_A_REVIEW_MESSAGE;

                return this.RedirectToAction("Details", "Courses", courseIdAsObject);
            }

            var reviewToCreate = new CreateReviewServiceModel
            {
                CourseId = courseId,
                UserId = userId,
                Content = content
            };

            await this.reviewService.CreateEntityAsync(reviewToCreate);

            return this.RedirectToAction("Details", "Courses", courseIdAsObject);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var review = await this.reviewService.GetByIdAsync<ReviewContentServiceModel>(id);

            if (review == null)
            {
                this.TempData[ERROR_NOTIFICATION] = INVALID_REVIEW_MESSAGE;

                return this.RedirectToAction("Index", "Home");
            }

            return this.View(review);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, int courseId, ReviewContentServiceModel review)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(review);
            }

            var isValid = await this.reviewService.IsAuthorOrAdminAsync(id, this.User);
            if (!isValid)
            {
                this.TempData[ERROR_NOTIFICATION] =
                    string.Format(NOT_HAVE_PERMISSION_MESSAGE, UPDATE);

                return this.RedirectToAction("Index", "Home");
            }

            await this.reviewService.UpdateEntityAsync(id, review);

            return this.RedirectToAction("Details", "Courses", new { Id = courseId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int reviewId, int courseId)
        {
            var review = await this.reviewService.GetByIdAsync<ReviewUserIdServiceModel>(reviewId);

            if (review == null)
            {
                this.TempData[ERROR_NOTIFICATION] = INVALID_REVIEW_MESSAGE;

                return this.RedirectToAction("Index", "Home");
            }

            var isValid = await this.reviewService.IsAuthorOrAdminAsync(reviewId, this.User);
            if (!isValid)
            {
                this.TempData[ERROR_NOTIFICATION] =
                    string.Format(NOT_HAVE_PERMISSION_MESSAGE, DELETE);

                return this.RedirectToAction("Index", "Home");
            }

            await this.reviewService.DeleteAsync(reviewId);

            return this.RedirectToAction("Details", "Courses", new { Id = courseId });
        }
    }
}
