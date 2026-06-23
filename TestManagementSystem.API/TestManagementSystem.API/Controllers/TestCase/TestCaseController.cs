using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TestManagementSystem.API.DTOs.TestCase;
using TestManagementSystem.API.Models;
using TestManagementSystem.API.Services.TestCase;

namespace TestManagementSystem.API.Controllers.TestCase
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TestCaseController : ControllerBase
    {
        private readonly ITestCaseService _testCaseService;

        public TestCaseController(ITestCaseService testCaseService)
        {
            _testCaseService = testCaseService;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetByProjectId(int projectId)
        {
            var testCases = await _testCaseService.GetByProjectIdAsync(projectId);
            return Ok(testCases);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var testCase = await _testCaseService.GetByIdAsync(id);
            if (testCase == null) return NotFound();
            return Ok(testCase);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTestCaseDto dto)
        {
            var testCase = await _testCaseService.CreateAsync(dto, GetUserId());
            return CreatedAtAction(nameof(GetById), new { id = testCase.Id }, testCase);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateTestCaseDto dto)
        {
            try
            {
                var testCase = await _testCaseService.UpdateAsync(id, dto, GetUserId());
                return Ok(testCase);
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
                await _testCaseService.DeleteAsync(id, GetUserId());
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
