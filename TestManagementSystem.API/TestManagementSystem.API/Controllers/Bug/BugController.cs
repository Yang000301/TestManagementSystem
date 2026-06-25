using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;     
using System.Security.Claims;
using TestManagementSystem.API.DTOs.Bug;
using TestManagementSystem.API.Services.Bug;
namespace TestManagementSystem.API.Controllers.Bug
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BugController : ControllerBase
    {
        private readonly IBugService _bugService;

        public BugController(IBugService bugService)
        {
            _bugService = bugService;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet("testcase/{testCaseId}")]
        public async Task<IActionResult> GetByTestCaseId(int testCaseId)
        {
            var bugs = await _bugService.GetByTestCaseIdAsync(testCaseId);
            return Ok(bugs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var bug = await _bugService.GetByIdAsync(id);
            if (bug == null) return NotFound();
            return Ok(bug);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBugDto dto)
        {
            var bug = await _bugService.CreateAsync(dto, GetUserId());
            return CreatedAtAction(nameof(GetById), new { id = bug.Id }, bug);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateBugDto dto)
        {
            try
            {
                var bug = await _bugService.UpdateAsync(id, dto, GetUserId());
                return Ok(bug);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _bugService.DeleteAsync(id, GetUserId());
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

}
