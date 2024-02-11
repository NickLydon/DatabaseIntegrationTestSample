using Database;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatabaseIntegrationTestSample.Controllers;

[ApiController]
[Route("[controller]")]
public class AttachmentController(SampleDbContext dbContext) : ControllerBase
{
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
        
        dbContext.Remove(new Attachment { Id = id });
        await dbContext.SaveChangesAsync();
        return NoContent();
    }
}