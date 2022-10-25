namespace StudentSystem.Services.Abstaction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Data.Models.Abstraction;
    using StudentSystem.Web.Data;

    public abstract class BaseService<TEntity> : IBaseService
        where TEntity : BaseModel
    {
        private const int MIN_PAGE_VALUE = 1; 

        protected BaseService(StudentSystemDbContext dbContext, IMapper mapper)
        {
            this.DbContext = dbContext;
            this.DbSet = this.DbContext.Set<TEntity>();
            this.Mapper = mapper;
        }

        protected StudentSystemDbContext DbContext { get; }

        protected DbSet<TEntity> DbSet { get; }

        protected IMapper Mapper { get; }

        public IQueryable<T> GetAllAsQueryable<T>(bool withDeleted = false)
        {
            var query = this.DbSet.AsQueryable();

            if (!withDeleted)
            {
                query = query
                    .Where(x => !x.IsDeleted);
            }

            return query.ProjectTo<T>(this.Mapper.ConfigurationProvider);
        }

        public IEnumerable<T> Paging<T>(IList<T> data, int currentPage, int entitiesPerPage)
        {
            var totalPages = Math.Ceiling(data.Count / (double)entitiesPerPage);

            if (currentPage < MIN_PAGE_VALUE)
            {
                currentPage = MIN_PAGE_VALUE;
            }
            else if(currentPage > totalPages)
            {
                currentPage = (int)totalPages;
            }

             return data
                .Skip((currentPage - 1) * entitiesPerPage)
                .Take(entitiesPerPage);
        }

        public async Task<T> GetByIdAsync<T>(int id) 
            => await this.DbSet
                .Where(x => x.Id == id)
                .ProjectTo<T>(this.Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

        public async Task CreateAsync<T>(T model)
        {
            var entity = this.Mapper.Map<TEntity>(model);

            entity.CreatedOn = DateTime.UtcNow;

            await this.DbSet.AddAsync(entity);
            await this.DbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync<T>(int id, T model)
        {
            var entityToUpdate = await this.DbSet.FindAsync(id);

            if (entityToUpdate == null)
            {
                return false;
            }

            this.Mapper.Map(model, entityToUpdate);
            entityToUpdate.ModifiedOn = DateTime.UtcNow;

            this.DbSet.Update(entityToUpdate);
            await this.DbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entityToDelete = await this.DbSet.FindAsync(id);

            if (entityToDelete == null)
            {
                return false;
            }

            entityToDelete.IsDeleted = true;
            entityToDelete.DeletedOn = DateTime.UtcNow;

            this.DbSet.Update(entityToDelete);
            await this.DbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsExistAsync(int id)
            => await this.DbSet.AnyAsync(x => x.Id == id);

        public async Task<int> GetCountAsync(bool withDeleted = false)
        {
            if (withDeleted)
            {
                return await this.DbSet.CountAsync();
            }

            return await this.DbSet
                .Where(x => x.IsDeleted == false)
                .CountAsync();
        }
    }
}
