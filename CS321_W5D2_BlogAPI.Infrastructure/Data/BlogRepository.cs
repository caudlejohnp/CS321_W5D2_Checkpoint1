using System;
using System.Collections.Generic;
using System.Linq;
using CS321_W5D2_BlogAPI.Core.Models;
using CS321_W5D2_BlogAPI.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace CS321_W5D2_BlogAPI.Infrastructure.Data
{
    public class BlogRepository : IBlogRepository
    {
        private readonly AppDbContext _dbContext;

        public BlogRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Blog> GetAll()
        {
            return _dbContext.Blogs
                .Include(b => b.Posts)
                .ThenInclude(p => p.Blog.User);
        }

        public Blog Get(int id)
        {
            return _dbContext.Blogs
                .Include(b => b.Posts)
                .ThenInclude(p => p.Blog.User)
                .SingleOrDefault(a => a.Id == id);
        }

        public Blog Add(Blog blog)
        {
            _dbContext.Add(blog);
            _dbContext.SaveChanges();
            return blog;
        }

        public Blog Update(Blog updatedItem)
        {
            var existingItem = _dbContext.Blogs.Find(updatedItem.Id);

            if (existingItem == null) return null;

            _dbContext.Entry(existingItem)
               .CurrentValues
               .SetValues(updatedItem);

            _dbContext.Blogs.Update(existingItem);
            _dbContext.SaveChanges();
            return existingItem;
        }

        public void Remove(int id)
        {
            var currentBlog = _dbContext.Blogs.FirstOrDefault(b => b.Id == id);
            if (currentBlog != null)
            {
                _dbContext.Blogs.Remove(currentBlog);
                _dbContext.SaveChanges();
            }
        }
    }
}
