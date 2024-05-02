using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonkeyBuisness.Models.Entity;
using MonkeyBuisness.Models.Filters.Task;
using MonkeyBuisness.Models.ViewModels.Note;
using MonkeyBuisness.Models.ViewModels.Task;
using MonkeyBuisness.Service.Implementations;
using MonkeyBuisness.Service.Interfaces;
using System.Threading.Tasks;

namespace MonkeyBuisness.Controllers;
public class TaskController : Controller
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskViewModel model)
    {
        string login = HttpContext.User.Identity.Name;

        var response = await _taskService.Create(model, login);
        if (response.StatusCode == Models.Enum.StatusCode.OK)
        {
            return Ok(new { description = response.Description });
        }
        return BadRequest(new { description = response.Description });
    }
    [HttpPost]
    public async Task<IActionResult> TaskHandler(TaskFilter filter)
    {
        string login = HttpContext.User.Identity.Name;
        var response = await _taskService.GetTasks(filter, login);
        if (response.StatusCode == Models.Enum.StatusCode.OK)
        {
            return Json(new { data = response.Data });
        }
        return BadRequest(new { description = "Failed to fetch tasks." });
        //return Json(new { data  = response.Data});
    }
    [Authorize]
    public IActionResult CreateTask()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> EndTask(long id)
    {
        var response = await _taskService.EndTask(id);
        if (response.StatusCode == Models.Enum.StatusCode.OK)
        {
            return Ok(new { description = response.Description });
        }
        return BadRequest(new { description = response.Description });
    }
    [HttpGet]
    public async Task<IActionResult> EditTask(long id)
    {

        var task = await _taskService.GetTaskById(id);
        if (task.Data == null)
        {
            return NotFound();
        }

        var model = new CreateTaskViewModel
        {
            Name = task.Data.Name,
            Priority = task.Data.Priority,
            Description = task.Data.Description,
            Id = task.Data.Id.ToString(),
        };

        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> EditTask(CreateTaskViewModel model)
    {
        var task = await _taskService.ChangeTask(model);
        if (task.Data == null)
        {
            return NotFound();
        }

        if (true)
        {
            return RedirectToAction("Index", "Task"); // Перенаправление, если редактирование прошло успешно
        }

    }
    public async Task<IActionResult> DeleteTask(long id)
    {

        var response = await _taskService.DeleteTask(id);
        if (response.StatusCode == Models.Enum.StatusCode.OK)
        {
            return RedirectToAction("Index", "Task");
            //return Ok(new { description = response.Description });
        }
        return BadRequest(new { description = response.Description });
    }
}
