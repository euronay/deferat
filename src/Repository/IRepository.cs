using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Deferat.Models;

namespace Deferat.Repository
{
    public interface IRepository<T> where T : IMetadata
    {
        T Get(string id);
        IEnumerable<T> Get (Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
    }
}