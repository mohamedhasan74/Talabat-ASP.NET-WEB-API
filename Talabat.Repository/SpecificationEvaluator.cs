using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    internal class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> MyQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var quer = inputQuery;
            if(specification.Criteria is not null)
                quer = quer.Where(specification.Criteria);
            if(specification.OrderBy is not null)
                quer = quer.OrderBy(specification.OrderBy);
            if(specification.OrderByDesc  is not null)
                quer = quer.OrderByDescending(specification.OrderByDesc);
            if (specification.IsPaginatined)
                quer = quer.Skip(specification.Skip).Take(specification.Take);
            quer = specification.Includes.Aggregate(quer, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
            return quer;
        }
    }
}
