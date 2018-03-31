using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FilmSearch.DAL
{
    public abstract class GenericRepository<TE>: IRepository<TE> where TE: class 
    {
        protected readonly FilmSearchContext _context;
        protected readonly DbSet<TE> _dbSet;

        public GenericRepository(FilmSearchContext context)
        {
            _context = context;
            _dbSet = context.Set<TE>();
        }

        public void Add(TE entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(object key)
        {
            var entity = _dbSet.Find(key);
            _dbSet.Remove(entity);
        }

        public IEnumerable<TE> GetAll()
        {
            return _dbSet.ToList();
        }

        public TE GetByKey(object key)
        {
            return _dbSet.Find(key);
        }

        public void Update(TE entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}