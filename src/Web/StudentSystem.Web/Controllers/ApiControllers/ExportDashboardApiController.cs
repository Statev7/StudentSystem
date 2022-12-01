namespace StudentSystem.Web.Controllers.ApiControllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Services.City;
    using StudentSystem.Services.Course;
    using StudentSystem.ViewModels.City;
    using StudentSystem.ViewModels.Course;

    [ApiController]
    [Route("api/exportDashboard")]
    public class ExportDashboardApiController : ControllerBase
    {
        private readonly ICourseService courseService;
        private readonly ICityService cityService;

        public ExportDashboardApiController(ICourseService courseService, ICityService cityService)
        {
            this.courseService = courseService;
            this.cityService = cityService;
        }

        [HttpGet]
        [Route("courses")]
        public async Task<IEnumerable<CourseIdNameViewModel>> GetCourses() 
            => await this.courseService
                .GetAllAsQueryable<CourseIdNameViewModel>()
                .OrderBy(c => c.Name)
                .ToListAsync();

        [HttpGet]
        [Route("cities")]
        public async Task<IEnumerable<CityIdNameViewModel>> GetCities()
            => await this.cityService
                .GetAllAsQueryable<CityIdNameViewModel>()
                .OrderBy(c => c.Name)
                .ToListAsync();
    }
}
