namespace Entities;

public class MessageAttachment
{
    public int Id { get; set; }
    public int MessageId { get; set; }
    public int AttachmentId { get; set; }

    public Message Message { get; set; }
    public Attachment Attachment { get; set; }
}