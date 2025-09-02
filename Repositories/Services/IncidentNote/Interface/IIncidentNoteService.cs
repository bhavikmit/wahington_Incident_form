using Microsoft.AspNetCore.Http;
using ViewModels.Incident;

public interface IIncidentNoteService
{
    Task<List<IncidentNoteViewModel>> GetNotesByIncidentId(long incidentId);
    Task AddNote(long incidentId, string author, string noteType, string content, string? fileUrl);
}