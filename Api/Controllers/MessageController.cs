using Database;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatabaseIntegrationTestSample.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController(SampleDbContext dbContext) : ControllerBase
{

    [HttpGet]
    public async Task<IEnumerable<Message>> Get()
    {
        return await dbContext.Messages.ToListAsync();
    }
    
    [HttpPost]
    public async Task<IActionResult> Upload(PostMessage message)
    {
        var attachments = await dbContext.Attachments
            .Where(x => message.AttachmentIds.Contains(x.Id))
            .ToListAsync();
        var created = await dbContext.Messages.AddAsync(new Message
        {
            Body = message.Body,
        });
        await dbContext.MessageAttachments.AddRangeAsync(attachments.Select(a => new MessageAttachment()
        {
            Attachment = a,
            Message = created.Entity
        }));
        await dbContext.SaveChangesAsync();
        return Ok(created.Entity);
    }
}