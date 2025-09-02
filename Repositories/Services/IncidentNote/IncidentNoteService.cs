using DataLibrary;
using ViewModels.Incident;
using Microsoft.EntityFrameworkCore;
public class IncidentNoteService : IIncidentNoteService
{
    private readonly ApplicationDbContext _db;

    public IncidentNoteService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<IncidentNoteViewModel>> GetNotesByIncidentId(long incidentId)
    {
        return await _db.IncidentNotes
            .Where(n => n.IncidentId == incidentId && !n.IsDeleted)
            .OrderByDescending(n => n.CreatedOn)
            .Select(n => new IncidentNoteViewModel
            {
                Id = n.Id,
                Author = n.Author,
                NoteType = n.NoteType,
                Content = n.Content,
                FileUrl = n.FileUrl,
                CreatedOn = n.CreatedOn
            })
            .ToListAsync();
    }

    public async Task AddNote(long incidentId, string author, string noteType, string content, string? fileUrl)
    {
        var note = new IncidentNote
        {
            IncidentId = incidentId,
            Author = author,
            NoteType = noteType,
            Content = content,
            FileUrl = fileUrl,
            CreatedOn = DateTime.Now,
            UpdatedOn = DateTime.Now,
            CreatedBy = 1, // TODO: replace with logged in user
            UpdatedBy = 1,
            ActiveStatus = Enums.ActiveStatus.Active,
            IsDeleted = false
        };

        _db.IncidentNotes.Add(note);
        await _db.SaveChangesAsync();
    }
}
