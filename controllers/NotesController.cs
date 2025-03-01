using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Repositories;
using NotesApi.Models;
using NotesApi.Dto;
using System.IdentityModel.Tokens.Jwt;

namespace NotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly NoteRepository _noteRepository;

        public NotesController(NoteRepository noteRepository, ILogger<NotesController> logger)
        {
            _noteRepository = noteRepository;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponseDto<Note>>> Get([FromQuery] NoteQueryParameters queryParameters)
        {   
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }
            var total = await _noteRepository.CountAllNotesAsync(queryParameters, userId.ToString());
            var data = await _noteRepository.GetAllNotesAsync(queryParameters, userId.ToString());
            var response = new PagedResponseDto<Note>(data, total);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> Get(int id)
        {   
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }
            var note = await _noteRepository.GetNoteByIdAsync(id, userId.ToString());
            if (note == null)
            {
                return NotFound();
            }
            return note;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] NoteDto createNoteDto)
        {   
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }
            var note = await _noteRepository.CreateNoteAsync(createNoteDto, userId.ToString());
            return CreatedAtAction(nameof(Get), new { id = note.Id }, note);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] NoteDto updateNoteDto)
        {   
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }
            var existingNote = await _noteRepository.GetNoteByIdAsync(id, userId.ToString());
            if (existingNote == null)
            {
                return NotFound();
            }
            await _noteRepository.UpdateNoteAsync(id, updateNoteDto, userId.ToString());
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {   
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }
            var existingNote = await _noteRepository.GetNoteByIdAsync(id, userId.ToString());
            if (existingNote == null)
            {
                return NotFound();
            }

            await _noteRepository.DeleteNoteAsync(id, userId.ToString());
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMany([FromBody] int[] ids)
        {   
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }
            foreach (var id in ids)
            {
                var existingNote = await _noteRepository.GetNoteByIdAsync(id, userId.ToString());
                if (existingNote == null)
                {
                    return NotFound($"Note with ID {id} not found.");
                }
            }
            await _noteRepository.DeleteManyNoteAsync(ids, userId.ToString());
            return NoContent();
        }

        private bool TryGetUserId(out string userId)
        {
            userId = null;
            var tokenPayload = HttpContext.Items["TokenPayload"] as JwtPayload;
            if (tokenPayload == null)
            {
                return false;
            }
            tokenPayload.TryGetValue("id", out var userIdObj);
            if (userIdObj == null)
            {
                return false;
            }
            userId = userIdObj.ToString();
            return true;
        }
    }
}