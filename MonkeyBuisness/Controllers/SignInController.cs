using Microsoft.AspNetCore.Mvc;
using MonkeyBuisness.Models.Filters.Task;
using MonkeyBuisness.Models.ViewModels.Person;
using MonkeyBuisness.Models.ViewModels.Task;
using MonkeyBuisness.Service.Implementations;
using MonkeyBuisness.Service.Interfaces;

namespace MonkeyBuisness.Controllers
{
    public class SignInController : Controller
    {

        private readonly IPersonService _personService;

        public SignInController(IPersonService personService)
        {
            _personService = personService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreatePersonViewModel model)
        {
            var response = await _personService.Create(model);
            if (response.StatusCode == Models.Enum.StatusCode.OK)
            {
                return Ok(new { description = response.Description });
            }
            return BadRequest(new { description = response.Description });
        }
        //[HttpPost]
        //public async Task<IActionResult> PersonHandler()
        //{
        //    var response = await _personService.GetTasks(filter);
        //    if (response.StatusCode == Models.Enum.StatusCode.OK)
        //    {
        //        return Json(new { data = response.Data });
        //    }
        //    return BadRequest(new { description = "Failed to fetch tasks." });
        //    //return Json(new { data  = response.Data});
        //}
    }
}
