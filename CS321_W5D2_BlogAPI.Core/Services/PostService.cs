using System;
using System.Collections.Generic;
using CS321_W5D2_BlogAPI.Core.Models;

namespace CS321_W5D2_BlogAPI.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IBlogRepository _blogRepository;
        private readonly IUserService _userService;

        public PostService(IPostRepository postRepository, IBlogRepository blogRepository, IUserService userService)
        {
            _postRepository = postRepository;
            _blogRepository = blogRepository;
            _userService = userService;
        }

        public Post Add(Post newPost)
        {
            var currentUserId = _userService.CurrentUserId;

            var blog = _blogRepository.Get(newPost.BlogId);

            if (currentUserId != blog.UserId)
            {
                throw new ApplicationException("You can not post to a blog that does not belong to you.");
            }
            newPost.DatePublished = DateTime.Now;
            return _postRepository.Add(newPost);
        }

        public Post Get(int id)
        {
            return _postRepository.Get(id);
        }

        public IEnumerable<Post> GetAll()
        {
            return _postRepository.GetAll();
        }
        
        public IEnumerable<Post> GetBlogPosts(int blogId)
        {
            return _postRepository.GetBlogPosts(blogId);
        }

        public void Remove(int id)
        {
            var post = this.Get(id);
            var currentUserId = _userService.CurrentUserId;
            var blog = _blogRepository.Get(post.BlogId);

            if (currentUserId != blog.UserId) // != post.Blog.UserId
            {
                throw new ApplicationException("You can not delete a post that does not belong to you.");
            }
            _postRepository.Remove(id);
        }

        public Post Update(Post updatedPost)
        {
            var currentUserId = _userService.CurrentUserId;

            var blog = _blogRepository.Get(updatedPost.BlogId);

            if (currentUserId != blog.UserId)
            {
                return null;
            }

            return _postRepository.Update(updatedPost);
        }
    }
}
