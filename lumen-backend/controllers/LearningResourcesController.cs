using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LumenAPI.Database;
using Microsoft.AspNetCore.Authorization;

namespace LumenAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/learningresources")]
    public class LearningResourcesController : ControllerBase
    {
        private readonly LearningResourceDb _db;

        public LearningResourcesController(LearningResourceDb db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLearningResource(int id)
        {
            var learningResource = await _db.LearningResources.FindAsync(id);
            if (learningResource is null)
            {
                return NotFound();
            }
            return Ok(learningResource);
        }

        [HttpGet]
        public async Task<IActionResult> GetLearningResources()
        {
            var learningResources = await _db.LearningResources.ToListAsync();
            return Ok(learningResources);
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateLearningResource([FromBody] LearningResource learningResource)
        {
            if (learningResource is null)
            {
                return BadRequest();
            }

            _db.LearningResources.Add(learningResource);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLearningResource), new { id = learningResource.Id }, learningResource);
        }

        [HttpPut("{id}")]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdateLearningResource(int id, [FromBody] LearningResource updatedLearningResource)
        {
            var learningResource = await _db.LearningResources.FindAsync(id);
            if (learningResource is null)
            {
                return NotFound();
            }

            learningResource.Title = updatedLearningResource.Title;
            learningResource.Description = updatedLearningResource.Description;
            learningResource.Url = updatedLearningResource.Url;

            await _db.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteLearningResource(int id)
        {
            var learningResource = await _db.LearningResources.FindAsync(id);
            if (learningResource is null)
            {
                return NotFound();
            }

            _db.LearningResources.Remove(learningResource);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("swap/{id1}/{id2}")]
        [Consumes("application/json")]
        public async Task<IActionResult> SwapLearningResources(int id1, int id2)
        {
            var learningResource1 = await _db.LearningResources.FindAsync(id1);
            var learningResource2 = await _db.LearningResources.FindAsync(id2);

            if (learningResource1 is null || learningResource2 is null)
            {
                return NotFound();
            }

            var tempTitle = learningResource1.Title;
            var tempDescription = learningResource1.Description;
            var tempUrl = learningResource1.Url;

            learningResource1.Title = learningResource2.Title;
            learningResource1.Description = learningResource2.Description;
            learningResource1.Url = learningResource2.Url;

            learningResource2.Title = tempTitle;
            learningResource2.Description = tempDescription;
            learningResource2.Url = tempUrl;

            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}