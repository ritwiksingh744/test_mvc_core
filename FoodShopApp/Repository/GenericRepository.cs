using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System;
using FoodShopApp.DataBaseContext;

namespace FoodShopApp.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private FoodShopContext _context = null;
        private DbSet<T> table = null;
        public GenericRepository(FoodShopContext context)
        {
            this._context = context;
            table = _context.Set<T>();
        }
        
        public T GetById(object id)
        {
            return table.Find(id);
        }
        public void Insert(T obj)
        {
            table.Add(obj);
        }
        public void Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public IQueryable<T> Search(Expression<Func<T, bool>> query)
        {
            return table.Where(query);
        }
    }
}
