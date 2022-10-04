namespace StudentSystem.Web.MappingConfiguration
{
    using AutoMapper;

	using StudentSystem.Data.Models.StudentSystem;
	using StudentSystem.Services.Review.Models;

	public class ReviewProfile : Profile
	{
		public ReviewProfile()
		{
			this.CreateMap<CreateReviewServiceModel, Review>();
			this.CreateMap<Review, ReviewUserIdServiceModel>();
			this.CreateMap<Review, ReviewFormServiceModel>().ReverseMap();
		}
	}
}
