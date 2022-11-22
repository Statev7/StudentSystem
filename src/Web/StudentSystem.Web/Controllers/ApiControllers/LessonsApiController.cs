namespace StudentSystem.Web.Controllers.ApiControllers
{
	using System.Threading.Tasks;

	using Microsoft.AspNetCore.Mvc;

	using StudentSystem.Services.Lesson;
	using StudentSystem.ViewModels.Lesson;

	[ApiController]
    [Route("api/lessons")]
    public class LessonsApiController : ControllerBase
	{
		private readonly ILessonService lessonService;

		public LessonsApiController(ILessonService lessonService)
		{
			this.lessonService = lessonService;
        }

		[HttpGet]
		public async Task<LessonDetailsViewModel> Get(int id)
		{
			var lesson = await
                this.lessonService.GetDetailsAsync(id);

			return lesson;
        }
	}
}
