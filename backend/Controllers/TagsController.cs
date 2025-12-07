using backend.Services.Interfaces;
using backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace backend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _service;
        private readonly ILogger<TagsController> _logger;

        public TagsController(ITagService service, ILogger<TagsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all tags");
            var tags = await _service.GetAllAsync();
            return Ok(tags);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Fetching tag with ID {Id}", id);
            var tag = await _service.GetByIdAsync(id);
            if (tag == null)
            {
                _logger.LogWarning("Tag not found with ID {Id}", id);
                return NotFound();
            }
            return Ok(tag);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TagDto dto)
        {
            _logger.LogInformation("Creating tag: {Name}", dto.Name);
            var created = await _service.CreateAsync(dto);
            _logger.LogInformation("Tag created with ID {Id}", created.Id);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TagDto dto)
        {
            _logger.LogInformation("Updating tag with ID {Id}", id);
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting tag with ID {Id}", id);
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
