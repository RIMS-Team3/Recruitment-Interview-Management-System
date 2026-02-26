using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.DTO;
using RecruitmentInterviewManagementSystem.Applications.Features.BookingInterviewSlot.Interfaces;

namespace RecruitmentInterviewManagementSystem.API.Controllers
{
    [Route("api/interview")]
    [ApiController]
    public class InterviewSlotController : ControllerBase
    {
        private readonly IViewListSlotInterviewRoleEmployer _interview;

        public InterviewSlotController(IViewListSlotInterviewRoleEmployer viewListSlotInterviewRoleEmployer)
        {
            _interview = viewListSlotInterviewRoleEmployer;
        }



        [HttpGet("slots")]
        public async Task<IActionResult> Index([FromQuery]RequestViewInterviewSlots request)
        {
            var interviewSlot = await _interview.Execute(request);

            return Ok(interviewSlot);
        }

    }
}
