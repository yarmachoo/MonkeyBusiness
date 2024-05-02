using Microsoft.EntityFrameworkCore;
using MonkeyBuisness.DAL.Interfaces;
using MonkeyBuisness.Models.Entity;
using MonkeyBuisness.Models.Enum;
using MonkeyBuisness.Models.Response;
using MonkeyBuisness.Models.ViewModels.Person;
using MonkeyBuisness.Models.ViewModels.Task;
using MonkeyBuisness.Service.Interfaces;

namespace MonkeyBuisness.Service.Implementations;
public class PersonService:IPersonService
{
    private readonly IBaseRepository<PersonEntity> _personRepository;
    private ILogger<PersonService> _logger;

    public PersonService(IBaseRepository<PersonEntity> personRepository, ILogger<PersonService> logger)
    {
        _logger = logger;
        _personRepository = personRepository;
    }

    public async Task<IBaseResponse<PersonEntity>> Create(CreatePersonViewModel model)
    {
        try
        {
            model.Validate();
            _logger.LogInformation($"Запрос на регистрацию пользователя: {model.Login}");

            var person = await _personRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Login == model.Login);
            if (person != null)
            {
                return new BaseResponse<PersonEntity>()
                {
                    Description = "Пользователь с таким Login уже есть!!",
                    StatusCode = StatusCode.TaskIsAlreadyExist
                };
            }

            person = new PersonEntity()
            {
                Login = model.Login,
                Password = model.Password,

            };
            await _personRepository.Create(person);

            _logger.LogInformation($"Новый пользователь зарегестрирован: {person.Login}");

            return new BaseResponse<PersonEntity>()
            {
                Description = $"Пользователь добавлен!",
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[PersonService.Create]: {ex.Message}");
            return new BaseResponse<PersonEntity>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalError
            };
        }

    }
}
