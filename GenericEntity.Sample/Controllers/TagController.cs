using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GenericEntity.Sample.Abstractions;
using GenericEntity.Sample.Entities;
using GenericEntity.Sample.Models.Tag;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GenericEntity.Sample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : Controller
    {
        private readonly ISampleDbContext _dbContext;

        public TagController(ISampleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddTagRequest request, CancellationToken ct)
        {
            var existTag = await _dbContext.Entity<Tag>()
                .Where(x => x.Name == request.Name)
                .Select(x => x.Id)
                .FirstOrDefaultAsync(ct);
            if (existTag != null)
                throw new Exception($"Tag {request.Name} is already exist");

            var newTag = new Tag
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            _dbContext.Entity<Tag>().Add(newTag);
            
            await _dbContext.SaveChangesAsync(ct);

            return new OkResult();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTagRequest request, CancellationToken ct) 
        {
            var tag = await _dbContext.Entity<Tag>().FindAsync(new[] { request.Id }, ct);
            if (tag is null)
                throw new Exception("Tag not found");
            
            tag.Name = request.Name;
            tag.UpdatedAt = DateTime.UtcNow;
            _dbContext.Entity<Tag>().Update(tag);

            await _dbContext.SaveChangesAsync(ct);

            return new OkResult();
        }

        [HttpGet]
        public async Task<IActionResult> Read(CancellationToken ct)
        {
            var tags = await _dbContext.Entity<Tag>()
                .Select(x => new GetTagResult
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync(ct);

            return new OkObjectResult(tags);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ReadDetail(string id, CancellationToken ct)
        {
            var tag = await _dbContext.Entity<Tag>()
                .Where(x => x.Id == id)
                .Select(x => new GetTagDetailResult
                {
                    Id = x.Id,
                    Name = x.Name,
                    TotalBlog = x.BlogTags.Count,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .FirstOrDefaultAsync(ct);
            if (tag is null)
                throw new Exception("Tag not found");

            return new OkObjectResult(tag);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken ct)
        {
            var tag = await _dbContext.Entity<Tag>().FindAsync(new[] { id }, ct);
            if (tag is null)
                throw new Exception("Tag not found");
            
            tag.IsActive = false;
            tag.UpdatedAt = DateTime.UtcNow;
            _dbContext.Entity<Tag>().Update(tag);

            await _dbContext.SaveChangesAsync(ct);

            return new OkResult();
        }
    }
}