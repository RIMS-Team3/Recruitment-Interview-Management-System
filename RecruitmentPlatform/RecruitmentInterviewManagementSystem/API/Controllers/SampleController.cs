using Microsoft.AspNetCore.Mvc;

namespace RecruitmentInterviewManagementSystem.API.Controllers
{
    public class SampleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
