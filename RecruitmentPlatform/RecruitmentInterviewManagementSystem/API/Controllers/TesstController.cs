using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentInterviewManagementSystem.Models;

namespace RecruitmentInterviewManagementSystem.API.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TesstController : ControllerBase
    {
        private readonly FakeTopcvContext _db;

        public TesstController(FakeTopcvContext fakeTopcvContext)
        {
            _db = fakeTopcvContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var list = await _db.JobPosts.ToListAsync();
            return Ok(list);
        }
    }
}