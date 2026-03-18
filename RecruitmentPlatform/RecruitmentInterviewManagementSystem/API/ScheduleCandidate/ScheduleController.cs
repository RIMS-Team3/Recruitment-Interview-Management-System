using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecruitmentInterviewManagementSystem.Applications.Features.Schedule;

namespace RecruitmentInterviewManagementSystem.API.ScheduleCandidate
{
    [Route("api/schedule")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IViewListScheduleForCandidate _viewSchedule;

        public ScheduleController(IViewListScheduleForCandidate viewListScheduleForCandidate)
        {
            _viewSchedule = viewListScheduleForCandidate;
        }


        [HttpGet("{Token}")]
        public async Task<IActionResult> ViewSchedule([FromRoute]Guid Token)
        {
            var schedules = await _viewSchedule.Execute(Token.ToString());

            return Ok(schedules);
        }
    }
}
