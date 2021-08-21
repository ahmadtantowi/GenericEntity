using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GenericEntity.Sample.Abstractions;
using GenericEntity.Sample.Entities;
using GenericEntity.Sample.Models.Blog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GenericEntity.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlogController : Controller
    {
        private readonly ISampleDbContext _dbContext;

        public BlogController(ISampleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddBlogRequest request, CancellationToken ct)
        {
            var blog = new Blog
            {
                Id = Guid.NewGuid().ToString(),
                Title = request.Title,
                Content = request.Content,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            _dbContext.Entity<Blog>().Add(blog);

            foreach (var tagId in request.TagIds)
            {
                var blogTag = new BlogTag
                {
                    Id = Guid.NewGuid().ToString(),
                    BlogId = blog.Id,
                    TagId = tagId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                _dbContext.Entity<BlogTag>().Add(blogTag);
            }
            
            await _dbContext.SaveChangesAsync(ct);

            return new OkResult();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateBlogRequest request, CancellationToken ct)
        {
            var blog = await _dbContext.Entity<Blog>()
                .Include(x => x.BlogTags)
                .FirstOrDefaultAsync(x => x.Id == request.Id, ct);
            if (blog is null)
                throw new Exception("Blog not found");

            blog.Title = request.Title;
            blog.Content = request.Content;
            blog.UpdatedAt = DateTime.UtcNow;
            _dbContext.Entity<Blog>().Update(blog);

            foreach (var blogTag in blog.BlogTags)
            {
                blogTag.IsActive = false;
                blog.UpdatedAt = DateTime.UtcNow;
                _dbContext.Entity<BlogTag>().Update(blogTag);
            }

            foreach (var tagId in request.TagIds)
            {
                var blogTag = new BlogTag
                {
                    Id = Guid.NewGuid().ToString(),
                    BlogId = blog.Id,
                    TagId = tagId,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };
                _dbContext.Entity<BlogTag>().Add(blogTag);
            }

            await _dbContext.SaveChangesAsync(ct);

            return new OkResult();
        }

        [HttpGet]
        public async Task<IActionResult> Read(CancellationToken ct)
        {
            var blogs = await _dbContext.Entity<Blog>()
                .Select(x => new GetBlogResult
                {
                    Id = x.Id,
                    Title = x.Title,
                    Content = x.Content,
                    Tags = x.BlogTags.Select(y => y.Tag.Name)
                })
                .ToListAsync(ct);

            return new OkObjectResult(blogs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ReadDetail(string id, CancellationToken ct)
        {
            var blog = await _dbContext.Entity<Blog>()
                .Where(x => x.Id == id)
                .Select(x => new GetBlogDetailResult
                {
                    Id = x.Id,
                    Title = x.Title,
                    Content = x.Content,
                    Tags = x.BlogTags.Select(y => new Models.Tag.GetTagResult
                    {
                        Id = y.TagId,
                        Name = y.Tag.Name
                    }),
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .FirstOrDefaultAsync(ct);
            if (blog is null)
                throw new Exception("Blog not found");

            return new OkObjectResult(blog);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken ct)
        {
            var blog = await _dbContext.Entity<Blog>()
                .Include(x => x.BlogTags)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
            if (blog is null)
                throw new Exception("Blog not found");

            blog.IsActive = false;
            blog.UpdatedAt = DateTime.UtcNow;
            _dbContext.Entity<Blog>();

            foreach (var blogTag in blog.BlogTags)
            {
                blogTag.IsActive = false;
                blogTag.UpdatedAt = DateTime.UtcNow;
                _dbContext.Entity<BlogTag>().Update(blogTag);
            }

            await _dbContext.SaveChangesAsync(ct);

            return new OkResult();
        }
    }
}