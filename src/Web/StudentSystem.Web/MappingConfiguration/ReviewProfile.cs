namespace StudentSystem.Web.MappingConfiguration
{
	using System.Linq;

	using AutoMapper;

	using StudentSystem.Data.Models.StudentSystem;
	using StudentSystem.Services.Review.Models;
	using StudentSystem.ViewModels.Review;

	public class ReviewProfile : Profile
	{
		public ReviewProfile()
		{
			this.CreateMap<CreateReviewServiceModel, Review>();
			this.CreateMap<Review, ReviewUserIdServiceModel>();
			this.CreateMap<Review, ReviewFormServiceModel>().ReverseMap();
			this.CreateMap<Review, ReviewForHomeViewModel>()
				.ForMember(d => d.UserFirstName, conf => conf.MapFrom(s => s.User.FirstName))
				.ForMember(d => d.UserLastName, conf => conf.MapFrom(s => s.User.LastName))
				.ForMember(d => d.UserImageUrl, conf => conf.MapFrom(s => s.User.ImageURL))
				.ForMember(d => d.UserRoleName, conf => conf.MapFrom(s => s.User.UserRoles.FirstOrDefault().Role.Name));
        }
	}
}
