﻿namespace StudentSystem.Web.Infrastructure.Helpers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using StudentSystem.Web.Data;
    using StudentSystem.Web.Infrastructure.Extensions;

    public static class Helper
    {

        public static async Task<bool> CheckIfInputDatesHasDiffThanCurrentOnesAsync(
            ValidationContext context, 
            DateTime startDate, 
            DateTime? endData = null)
        {
            var dbContext = context.GetRequiredService<StudentSystemDbContext>();
            var httpContextAccessor = (IHttpContextAccessor)context.GetService(typeof(IHttpContextAccessor));

            var isDifferent = false;
            var entityId = GetEntityIdFromRoute(httpContextAccessor);
            var objectTypeName = context.ObjectInstance.GetType().Name;

            if (objectTypeName == "LessonFormServiceModel")
            {
                var lesson = await dbContext.Lessons
                        .FirstOrDefaultAsync(x => x.Id == entityId);

                isDifferent = lesson.Begining.TrimToSeconds() != startDate.TrimToSeconds() ||
                              endData.HasValue && 
                              lesson.End.TrimToSeconds() != endData.Value.TrimToSeconds();
            }
            else if (objectTypeName == "CourseFormServiceModel")
            {
                var course = await dbContext.Courses
                        .FirstOrDefaultAsync(x => x.Id == entityId);

                isDifferent = course.StartDate.TrimToSeconds() != startDate.TrimToSeconds() ||
                              endData.HasValue &&
                              course.EndDate.TrimToSeconds() != endData.Value.TrimToSeconds();
            }

            return isDifferent;
        }

        private static int GetEntityIdFromRoute(IHttpContextAccessor httpContextAccessor)
        {
            var idAsObject = httpContextAccessor.HttpContext.GetRouteValue("id");
            var id = int.Parse(idAsObject.ToString());

            return id;
        }
    }
}
