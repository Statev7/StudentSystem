﻿namespace StudentSystem.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Services.Course;
    using StudentSystem.Services.Course.Models;
    using StudentSystem.Services.Lesson;
    using StudentSystem.Services.Lesson.Models;
    using StudentSystem.ViewModels.Course;
    using StudentSystem.Web.Areas.Trainings.Controllers.Abstraction;

    using static StudentSystem.Web.Common.GlobalConstants;
    using static StudentSystem.Web.Common.NotificationsConstants;

    [Authorize(Roles = ADMIN_ROLE)]
    public class LessonsController : TrainingController
    {
        private const int LESSON_PER_PAGE = 6;

        private readonly ILessonService lessonService;
        private readonly ICourseService courseService;

        public LessonsController(
            ILessonService lessonService,
            ICourseService courseService)
        {
            this.lessonService = lessonService;
            this.courseService = courseService;
        }

        [HttpGet]
        [Authorize(Roles = ADMIN_ROLE)]
        public async Task<IActionResult> Index(int[] filters, int currentPage = 1)
        {
            var lessons = await this
                .lessonService
                .GetAllLessonsPagedAsync(filters, currentPage, LESSON_PER_PAGE);

            return View(lessons);
        }

        [HttpGet]
        [Authorize(Roles = ADMIN_ROLE)]
        public async Task<IActionResult> Create()
        {
            var lesson = new LessonFormServiceModel()
            {
                Begining = null,
                End = null,
                Courses = await this.GetAllCoursesSorted()
            };

            return this.View(lesson);
        }

        [HttpPost]
        [Authorize(Roles = ADMIN_ROLE)]
        public async Task<IActionResult> Create(LessonFormServiceModel lesson)
        {
            if (!this.ModelState.IsValid)
            {
                lesson.Courses = await this.GetAllCoursesSorted();

                return this.View(lesson);
            }

            var errorMessage = await this.ValidateLessonDatesAgainstCourseDatesAsync(lesson);
            if (errorMessage != string.Empty)
            {
                this.TempData[ERROR_NOTIFICATION] = errorMessage;

                lesson.Courses = await this.GetAllCoursesSorted();
                return this.View(lesson);
            }

            await this.lessonService.CreateEntityAsync(lesson);

            this.TempData[SUCCESS_NOTIFICATION] =
                string.Format(SUCCESSFULLY_CREATED_ENTITY_MESSAGE, lesson.Title, LESSON_KEYWORD);

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var lesson = await this.lessonService.GetByIdAsync<LessonFormServiceModel>(id);

            if (lesson == null)
            {
                this.TempData[ERROR_NOTIFICATION] =
                    string.Format(SUCH_A_ENTITY_DOES_NOT_EXIST, LESSON_KEYWORD);

                return this.RedirectToAction(nameof(this.Index));
            }

            lesson.Courses = await this.courseService.GetAllAsync<CourseIdNameViewModel>();

            return this.View(lesson);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, LessonFormServiceModel lesson)
        {
            if (!this.ModelState.IsValid)
            {
                lesson.Courses = await this.GetAllCoursesSorted();

                return this.View(lesson);
            }

            var errorMessage = await this.ValidateLessonDatesAgainstCourseDatesAsync(lesson);
            if (errorMessage != string.Empty)
            {
                this.TempData[ERROR_NOTIFICATION] = errorMessage;

                lesson.Courses = await this.GetAllCoursesSorted();
                return this.View(lesson);
            }

            var isUpdated = await this.lessonService.UpdateEntityAsync(id, lesson);
            if (!isUpdated)
            {
                this.TempData[ERROR_NOTIFICATION] =
                    string.Format(SUCH_A_ENTITY_DOES_NOT_EXIST, LESSON_KEYWORD);

                return this.RedirectToAction(nameof(this.Index));
            }

            this.TempData[SUCCESS_NOTIFICATION] =
                string.Format(SUCCESSFULLY_UPDATED_ENTITY_MESSAGE, lesson.Title, LESSON_KEYWORD);

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var isLessonExist = await this.lessonService.IsExistAsync(id);
            if (!isLessonExist)
            {
                this.TempData[ERROR_NOTIFICATION] =
                    string.Format(SUCH_A_ENTITY_DOES_NOT_EXIST, LESSON_KEYWORD);

                return this.RedirectToAction(nameof(this.Index));
            }

            await this.lessonService.DeleteAsync(id);
            this.TempData[SUCCESS_NOTIFICATION] = SUCCESSFULLY_DELETED_ENTITY_MESSAGE;

            return this.RedirectToAction(nameof(this.Index));
        }

        private async Task<IEnumerable<CourseIdNameViewModel>> GetAllCoursesSorted()
            => await this.courseService
            .GetAllAsQueryable<CourseIdNameViewModel>()
            .OrderBy(x => x.Name)
            .ToListAsync();

        private async Task<string> ValidateLessonDatesAgainstCourseDatesAsync(LessonFormServiceModel lesson)
        {
            var isCourseExist = await this.courseService.IsExistAsync(lesson.CourseId);
            if (!isCourseExist)
            {
                var errorMessage =
                    string.Format(SUCH_A_ENTITY_DOES_NOT_EXIST, COURSE_KEYWORD);

                return errorMessage;
            }

            var course = await this.courseService.GetByIdAsync<CourseDatesServiceModel>(lesson.CourseId);

            //Check if lesson start/end date is earlier/later than the course start/end date
            var isDatesInvalid = lesson.Begining.Value.Date < course.StartDate.Date ||
                                 lesson.End.Value.Date > course.EndDate.Date;
            if (isDatesInvalid)
            {
                var errorMessage = 
                    string.Format(DATES_CANNOT_BE_EARLIER_OR_LATER_THAN_COURSE_DATES_MESSAGE,
                    nameof(lesson.Begining), nameof(lesson.End), nameof(course.StartDate), nameof(course.EndDate));

                return errorMessage;
            }

            return string.Empty;
        }
    }
}