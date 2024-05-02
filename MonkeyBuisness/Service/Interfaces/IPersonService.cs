using MonkeyBuisness.Models.Entity;
using MonkeyBuisness.Models.Filters.Task;
using MonkeyBuisness.Models.Response;
using MonkeyBuisness.Models.ViewModels.Person;
using MonkeyBuisness.Models.ViewModels.Task;

namespace MonkeyBuisness.Service.Interfaces;
public interface IPersonService
{
    Task<IBaseResponse<PersonEntity>> Create(CreatePersonViewModel model);

    //Task<IBaseResponse<IEnumerable<PersonEntity>>> GetTasks(TaskFilter filter);
    //Task<IBaseResponse<bool>> EndTask(long id);
}