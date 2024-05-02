using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MonkeyBuisness.DAL.Interfaces;
using MonkeyBuisness.Models.Entity;
using MonkeyBuisness.Models.Enum;
using MonkeyBuisness.Models.Extensions;
using MonkeyBuisness.Models.Filters.Task;
using MonkeyBuisness.Models.Response;
using MonkeyBuisness.Models.ViewModels.Task;
using MonkeyBuisness.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using MonkeyBuisness.DAL.Repositories;
using MonkeyBuisness.Models.ViewModels.Task;

namespace MonkeyBuisness.Service.Implementations;
public class TaskService : ITaskService
{

    private readonly IBaseRepository<TaskEntity> _taskRepository;
    private readonly IBaseRepository<PersonEntity> _personRepository;
    private ILogger<TaskService> _logger;

    public TaskService(IBaseRepository<TaskEntity> taskRepository, IBaseRepository<PersonEntity> personRepository, ILogger<TaskService> logger)
    {
        _logger = logger;
        _taskRepository = taskRepository;
        _personRepository = personRepository;
    }

    public async Task<IBaseResponse<TaskEntity>> Create(CreateTaskViewModel model, string login)
    {
        try
        {
            model.Validate();
            _logger.LogInformation($"Запрос на создание задачи: {model.Name}");

            var task = await _taskRepository.GetAll()
                .Where(x => x.Created.Date == DateTime.Today)
                .FirstOrDefaultAsync(x => x.Name == model.Name);
            if (task != null)
            {
                return new BaseResponse<TaskEntity>()
                {
                    Description = "Задача с таким названием уже есть!!",
                    StatusCode = StatusCode.TaskIsAlreadyExist
                };
            }

            long id = 0;
            var person = await _personRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Login == login);
            if (person != null)
            {
                id = person.Id;
            }

            task = new TaskEntity()
            {
                Name = model.Name,
                Description = model.Description,
                Priority = model.Priority,
                Created = DateTime.Now,
                IsDone = false,
                PersonId = id
            };
            
            await _taskRepository.Create(task);
            _logger.LogInformation($"Задача создана: {task.Name} {task.Created}");

            var personToAddTask = await _personRepository.GetAll()
            .Include(p => p.Tasks) // Включаем список задач пользователя
            .FirstOrDefaultAsync(x => x.Login == login);

            // Добавляем созданную задачу в список задач пользователя
            personToAddTask.Tasks.Add(task);

            await _personRepository.Update(person);

            _logger.LogInformation($"Задача добавлена в список пользовательских задачи: {personToAddTask.Login}");



            return new BaseResponse<TaskEntity>()
            {
                Description = $"задача создана!",
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[TaskService.Create]: {ex.Message}");
            return new BaseResponse<TaskEntity>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalError
            };
        }

    }
    public async Task<IBaseResponse<bool>> EndTask(long id)
    {
        try
        {

            var task = await _taskRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (task == null)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.TaskNotFound,
                    Description = "Задача не найдена!"
                };
            }
            task.IsDone = true;

            await _taskRepository.Update(task);

            return new BaseResponse<bool>()
            {
                StatusCode = StatusCode.OK,
                Description = "Задача завершена!"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[TaskService.Create]: {ex.Message}");
            return new BaseResponse<bool>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalError
            };
        }
    }

    public async Task<IBaseResponse<IEnumerable<TaskViewModel>>> GetTasks(TaskFilter filter, string login)
    {
        try
        {
            long id = 0;
            var person = await _personRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Login == login);
            if (person != null)
            {
                id = person.Id;
            }

            var tasks = await _taskRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(filter.Name), x => x.Name == filter.Name)
                .WhereIf(filter.Priority.HasValue, x => x.Priority == filter.Priority && x.PersonId == id)
                .Select(x => new TaskViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsDone = x.IsDone == true ? "Готова" : "Не готова",
                    Priority = x.Priority.GetDisplayName(),
                    Created = x.Created.ToLongDateString()
                })
                .ToListAsync();

            return new BaseResponse<IEnumerable<TaskViewModel>>()
            {
                Data = tasks,
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[TaskService.Create]: {ex.Message}");
            return new BaseResponse<IEnumerable<TaskViewModel>>()
            {
                Description = $"{ex.Message}",
                StatusCode = StatusCode.InternalError
            };
        }
    }
    public async Task<IBaseResponse<TaskEntity>> GetTaskById(long id)
    {
        try
        {
            var task = await _taskRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (task == null)
            {
                return new BaseResponse<TaskEntity>()
                {
                    Description = "Конспекта с таким id нет!",
                    StatusCode = StatusCode.TaskIsAlreadyExist
                };
            }

            return new BaseResponse<TaskEntity>()
            {
                StatusCode = StatusCode.OK,
                Description = "Книга изменена!",
                Data = task
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[TaskService.Create]: {ex.Message}");
            return new BaseResponse<TaskEntity>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalError
            };
        }
    }

    public async Task<IBaseResponse<TaskEntity>> ChangeTask(CreateTaskViewModel model)
    {
        try
        {
            long id = long.Parse(model.Id);
            var task = await _taskRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (task == null)
            {
                return new BaseResponse<TaskEntity>()
                {
                    Description = "Книги с таким id нет!",
                    StatusCode = StatusCode.TaskIsAlreadyExist
                };
            }

            TaskEntity newTask = new TaskEntity()
            {

                PersonId = task.PersonId,
                Name = model.Name,
                Priority = task.Priority,
                Description = model.Description,
                Created = task.Created,
                IsDone = task.IsDone,
                Person = task.Person
            };
            await _taskRepository.Delete(task);
            await _taskRepository.Create(newTask);

            return new BaseResponse<TaskEntity>()
            {
                StatusCode = StatusCode.OK,
                Description = "Книга изменена!",
                Data = newTask
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[TaskService.Create]: {ex.Message}");
            return new BaseResponse<TaskEntity>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalError
            };
        }
    }
    public async Task<IBaseResponse<bool>> DeleteTask(long id)
    {
        try
        {

            var movie = await _taskRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.TaskNotFound,
                    Description = "Задача не найдена!"
                };
            }

            await _taskRepository.Delete(movie);

            return new BaseResponse<bool>()
            {
                StatusCode = StatusCode.OK,
                Description = "Конспект удален!"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[TaskService.Create]: {ex.Message}");
            return new BaseResponse<bool>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalError
            };
        }
    }
}
