using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System;

namespace FoodShopApp.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(object id);
        void Insert(T obj);
        void Update(T obj);
        void Save();
        IQueryable<T> Search(Expression<Func<T, bool>> query);
    }
}
