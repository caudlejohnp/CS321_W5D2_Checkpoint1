using System;
using System.Collections.Generic;
using System.Linq;
using CS321_W5D2_BlogAPI.Core.Models;
using CS321_W5D2_BlogAPI.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace CS321_W5D2_BlogAPI.Infrastructure.Data
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _dbContext;
        public PostRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public Post Get(int id)
        {
            return _dbContext.Posts
                .Include(a => a.Blog.User)
                .Include(a => a.Blog)
                .SingleOrDefault(a => a.Id == id);
        }

        public IEnumerable<Post> GetBlogPosts(int blogId)
        {
            var allBlogs = _dbContext.Posts
                .Include(p => p.Blog.User)
                .Include(p => p.Blog)
                .Where(p => p.BlogId == blogId);
            return allBlogs;
        }

        public Post Add(Post post)
        {
            _dbContext.Add(post);
            _dbContext.SaveChanges();
            return post;
        }

        public Post Update(Post post)
        {
            var existingPost = _dbContext.Posts.Find(post.Id);

            if (existingPost == null) return null;

            _dbContext.Entry(existingPost)
                .CurrentValues
                .SetValues(post);

            _dbContext.Posts.Update(post);
            _dbContext.SaveChanges();
            return post;
        }

        public IEnumerable<Post> GetAll()
        {
            return _dbContext.Posts
                .Include(a => a.Blog.Name)
                .ToList();
        }

        public void Remove(int id)
        {
            var currentPost = _dbContext.Posts.FirstOrDefault(p => p.Id == id);

            if (currentPost != null)
            {
                _dbContext.Posts.Remove(currentPost);
                _dbContext.SaveChanges();
            }
        }
    }
}
