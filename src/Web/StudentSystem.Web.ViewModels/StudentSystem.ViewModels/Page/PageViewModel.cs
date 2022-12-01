namespace StudentSystem.ViewModels.Page
{
    using System;
    using System.Collections.Generic;

    public class PageViewModel
    {
        public int CurrentPage { get; set; }

        public int EntitiesPerPage { get; set; }

        public int TotalEntities{ get; set; }

        public int TotalPages => (int)Math.Ceiling(this.TotalEntities / (double)this.EntitiesPerPage);

        public bool HasPrevious => this.CurrentPage - 1 >= 1;

        public bool HasNext => this.TotalPages > CurrentPage;

        public int PreviousPage => this.CurrentPage - 1;

        public int NextPage => this.CurrentPage + 1;

        public int[] Filters { get; set; }

        public string Search { get; set; }
    }
}
