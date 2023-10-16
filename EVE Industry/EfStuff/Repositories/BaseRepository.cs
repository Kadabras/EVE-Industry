using EVE_Industry.EfStuff.DbModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace EVE_Industry.EfStuff.Repositories
{
    public abstract class BaseRepository<Template> where Template : BaseModel
    {
        protected WebContext _webContext;
        protected DbSet<Template> _dbSet;

        public BaseRepository(WebContext webContext)
        {
            _webContext = webContext;
            _dbSet = webContext.Set<Template>();
        }

        public virtual Template Get(long id)
        {
            return _dbSet.SingleOrDefault(x => x.Id == id);
        }

        public virtual List<Template> GetRange(int startId, int endId)
        {
            return GetAll().GetRange(startId, endId - startId);
        }

        protected virtual IQueryable<Template> GetAllQueryable()
        {
            return _dbSet;
        }


        public virtual List<Template> GetAll()
        {
            return _dbSet.ToList();
        }

        public virtual void Save(Template model)
        {
            if (model.Id > 0)
            {
                _webContext.Update(model);
            }
            else
            {
                _dbSet.Add(model);
            }
            _webContext.SaveChanges();
        }

        public virtual void Remove(long id)
        {
            Remove(Get(id));
        }

        public virtual void Remove(Template model)
        {
            _dbSet.Remove(model);
            _webContext.SaveChanges();
        }

        public virtual void Remove(List<Template> models)
        {
            foreach (Template model in models)
            {
                _dbSet.Remove(model);
                _webContext.SaveChanges();
            }
        }

        public virtual int Count(bool getRemovedRecord = false)
            => _dbSet.Count();

        public List<Template> GetForPagination(int perPage, int page)
            => _dbSet
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .ToList();

    }


}
