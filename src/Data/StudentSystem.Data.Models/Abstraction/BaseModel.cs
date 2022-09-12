namespace StudentSystem.Data.Models.Abstraction
{
    using System;

    public class BaseModel<TEntity>
    {
        public TEntity Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
