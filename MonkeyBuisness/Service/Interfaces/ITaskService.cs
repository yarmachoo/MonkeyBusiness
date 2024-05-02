using MonkeyBuisness.Models.Entity;
using MonkeyBuisness.Models.Filters.Task;
using MonkeyBuisness.Models.Response;
using MonkeyBuisness.Models.ViewModels.Movie;
using MonkeyBuisness.Models.ViewModels.Task;

namespace MonkeyBuisness.Service.Interfaces;
public interface ITaskService
{
    Task<IBaseResponse<TaskEntity>> Create(CreateTaskViewModel model, string login);

    Task<IBaseResponse<IEnumerable<TaskViewModel>>> GetTasks(TaskFilter filter, string login);
    Task<IBaseResponse<bool>> EndTask(long id);
    Task<IBaseResponse<TaskEntity>> ChangeTask(CreateTaskViewModel model);
    Task<IBaseResponse<bool>> DeleteTask(long id);
    Task<IBaseResponse<TaskEntity>> GetTaskById(long id);
}
