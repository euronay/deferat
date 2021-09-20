using Deferat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Deferat.Repository
{
    public interface IRepository<T> where T : IMetadata
    {
        string BasePath { get; }
        T Get(string id);
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
    }
}