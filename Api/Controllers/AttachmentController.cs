using Database;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DatabaseIntegrationTestSample.Controllers;

[ApiController]
[Route("[controller]")]
public class AttachmentController(SampleDbContext dbContext) : ControllerBase
{
    private const int ConstraintViolationErrorCode = 547;

    [HttpPost]
    public async Task<IActionResult> Upload(string url)
    {
        var attachment = await dbContext.Attachments.AddAsync(new Attachment
        {
            Url = url
        });
        await dbContext.SaveChangesAsync();
        return Ok(attachment.Entity);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        if (await dbContext.MessageAttachments.AnyAsync(m => m.AttachmentId == id) ||
         await dbContext.BlogPostAttachments.AnyAsync(m => m.AttachmentId == id))
            return BadRequest($"Attachment {id} is referenced by a message and cannot be deleted");
        
        try
        {
            dbContext.Remove(new Attachment { Id = id });
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateException e)
            when (e.InnerException is SqlException ex && ex.Errors.Cast<SqlError>().Any(x => x.Number == ConstraintViolationErrorCode))
        {
            return BadRequest($"Attachment {id} is referenced by a message and cannot be deleted");
        }
    }
}