using Microsoft.EntityFrameworkCore;
using MonkeyBuisness.DAL.Interfaces;
using MonkeyBuisness.DAL.Repositories;
using MonkeyBuisness.Models.Entity;
using MonkeyBuisness.Models.Enum;
using MonkeyBuisness.Models.Extensions;
using MonkeyBuisness.Models.Filters.Note;
using MonkeyBuisness.Models.Filters.Task;
using MonkeyBuisness.Models.Response;
using MonkeyBuisness.Models.ViewModels.Movie;
using MonkeyBuisness.Models.ViewModels.Note;
using MonkeyBuisness.Models.ViewModels.Task;
using MonkeyBuisness.Service.Interfaces;

namespace MonkeyBuisness.Service.Implementations;
public class NoteService:INoteService
{
    private readonly IBaseRepository<NoteEntity> _noteRepository;
    private readonly IBaseRepository<PersonEntity> _personRepository;
    private ILogger<NoteService> _logger;

    public NoteService(IBaseRepository<NoteEntity> noteRepository, IBaseRepository<PersonEntity> personRepository, ILogger<NoteService> logger)
    {
        _logger = logger;
        _noteRepository = noteRepository;
        _personRepository = personRepository;
    }

    public async Task<IBaseResponse<NoteEntity>> Create(CreateNoteViewModel model, string login)
    {
        try
        {
            model.Validate();
            _logger.LogInformation($"Запрос на создание конспекта: {model.Name}");

            var note = await _noteRepository.GetAll()
                .Where(x => x.Created.Date == DateTime.Today)
                .FirstOrDefaultAsync(x => x.Name == model.Name && x.Subject==model.Subject && x.Theme == model.Theme);

            if (note != null)
            {
                return new BaseResponse<NoteEntity>()
                {
                    Description = "Конспект с таким же названием, темой и по этому предмету уже есть!!",
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
            model.Subject = "jhjhj";
            note = new NoteEntity()
            {
                Name = model.Name,
                Subject = model.Subject,
                Theme = model.Theme,
                Created = DateTime.Now,
                Note = model.Note,
                PersonId = id
            };

            await _noteRepository.Create(note);
            _logger.LogInformation($"Задача создана: {note.Name} {note.Created}");

            var personToAddNote = await _personRepository.GetAll()
            .Include(p => p.Notes) // Включаем список задач пользователя
            .FirstOrDefaultAsync(x => x.Login == login);

            // Добавляем созданную задачу в список задач пользователя
            personToAddNote.Notes.Add(note);

            await _personRepository.Update(person);

            _logger.LogInformation($"Конспект добавлен в список пользовательских задач: {personToAddNote.Login}");



            return new BaseResponse<NoteEntity>()
            {
                Description = $"Конспект создан!",
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[NoteService.Create]: {ex.Message}");
            return new BaseResponse<NoteEntity>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalError
            };
        }

    }

    public async Task<IBaseResponse<IEnumerable<NoteViewModel>>> GetNotes(NoteFilter filter, string login)
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

            var notes = await _noteRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(filter.Name), x => x.Name == filter.Name && x.PersonId == id)
                .WhereIf(!string.IsNullOrWhiteSpace(filter.Subject), x => x.Subject == filter.Subject)
                .WhereIf(!string.IsNullOrWhiteSpace(filter.Theme), x => x.Theme == filter.Theme)
                .WhereIf(!string.IsNullOrWhiteSpace(filter.Note), x => x.Note == filter.Note)
                .Select(x => new NoteViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Subject = x.Subject,
                    Theme = x.Theme,
                    Note = x.Note,
                    Created = x.Created.ToLongDateString(),
                })
                .ToListAsync();

            return new BaseResponse<IEnumerable<NoteViewModel>>()
            {
                Data = notes,
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[NoteService.Create]: {ex.Message}");
            return new BaseResponse<IEnumerable<NoteViewModel>>()
            {
                Description = $"{ex.Message}",
                StatusCode = StatusCode.InternalError
            };
        }
    }
    public async Task<IBaseResponse<NoteEntity>> GetNoteById(long id)
    {
        try
        {
            var note = await _noteRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (note == null)
            {
                return new BaseResponse<NoteEntity>()
                {
                    Description = "Конспекта с таким id нет!",
                    StatusCode = StatusCode.TaskIsAlreadyExist
                };
            }

            return new BaseResponse<NoteEntity>()
            {
                StatusCode = StatusCode.OK,
                Description = "Книга изменена!",
                Data = note
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[MovieService.Create]: {ex.Message}");
            return new BaseResponse<NoteEntity>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalError
            };
        }
    }

    public async Task<IBaseResponse<NoteEntity>> ChangeNote(CreateNoteViewModel model)
    {
        try
        {
            long id = long.Parse(model.Id);
            var note = await _noteRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (note == null)
            {
                return new BaseResponse<NoteEntity>()
                {
                    Description = "Книги с таким id нет!",
                    StatusCode = StatusCode.TaskIsAlreadyExist
                };
            }

            NoteEntity newNote = new NoteEntity()
            {
                PersonId = note.PersonId,
                Name = model.Name,
                Subject = model.Subject,
                Note = model.Note,
                Theme = note.Theme,
                Person = note.Person,
            };
            await _noteRepository.Delete(note);
            await _noteRepository.Create(newNote);

            return new BaseResponse<NoteEntity>()
            {
                StatusCode = StatusCode.OK,
                Description = "Книга изменена!",
                Data = newNote
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[MovieService.Create]: {ex.Message}");
            return new BaseResponse<NoteEntity>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalError
            };
        }
    }
    public async Task<IBaseResponse<bool>> DeleteNote(long id)
    {
        try
        {

            var movie = await _noteRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.TaskNotFound,
                    Description = "Задача не найдена!"
                };
            }

            await _noteRepository.Delete(movie);

            return new BaseResponse<bool>()
            {
                StatusCode = StatusCode.OK,
                Description = "Конспект удален!"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[NoteService.Create]: {ex.Message}");
            return new BaseResponse<bool>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.InternalError
            };
        }
    }
}
