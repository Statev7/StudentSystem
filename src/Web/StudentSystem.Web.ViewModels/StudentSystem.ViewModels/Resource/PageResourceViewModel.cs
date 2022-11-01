namespace StudentSystem.ViewModels.Resource
{
    using System.Collections.Generic;

    using StudentSystem.ViewModels.Page;

    public class PageResourceViewModel : PageViewModel
	{
		public IEnumerable<EntityForPageViewModel> Resources { get; set; }
	}
}
