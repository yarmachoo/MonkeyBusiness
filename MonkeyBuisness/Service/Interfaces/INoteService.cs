using MonkeyBuisness.Models.Entity;
using MonkeyBuisness.Models.Filters.Note;
using MonkeyBuisness.Models.Response;
using MonkeyBuisness.Models.ViewModels.Movie;
using MonkeyBuisness.Models.ViewModels.Note;

namespace MonkeyBuisness.Service.Interfaces;
public interface INoteService
{
    Task<IBaseResponse<NoteEntity>> Create(CreateNoteViewModel model, string login);

    Task<IBaseResponse<IEnumerable<NoteViewModel>>> GetNotes(NoteFilter filter, string login);
    //Task<IBaseResponse<bool>> EndTask(long id);
    Task<IBaseResponse<NoteEntity>> ChangeNote(CreateNoteViewModel model);
    Task<IBaseResponse<NoteEntity>> GetNoteById(long id);
    Task<IBaseResponse<bool>> DeleteNote(long id);
}
