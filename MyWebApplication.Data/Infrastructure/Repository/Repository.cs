﻿using Microsoft.EntityFrameworkCore;
using MyWebApplication.Data.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.Data.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDBContext _context;
        private DbSet<T> _dbSet;
        public Repository(ApplicationDBContext context)
        {
                _context = context;
                _dbSet = _context.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbSet?.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entity)
        {
            _dbSet.RemoveRange(entity);
        }

        public IEnumerable<T> GetAll(string ? includeProperties=null)
        {
            IQueryable<T> query = _dbSet;
            if(includeProperties != null)
            {
                foreach(var item in includeProperties.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries)) 
                {
                 query=query.Include(item);
                }
            }
           return query.ToList();
        }

        public T GetT(Expression<Func<T, bool>> predicate, string? includeProperties=null)
        {
            IQueryable<T> query = _dbSet;
            query=query.Where(predicate);
            if (includeProperties != null)
            {
                foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.FirstOrDefault();
            //return _dbSet.Where(predicate).FirstOrDefault();
        }
    }
}
