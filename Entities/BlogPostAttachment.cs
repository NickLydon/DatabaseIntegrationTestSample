namespace Entities;

public class BlogPostAttachment
{
    public int Id { get; set; }
    public int BlogPostId { get; set; }
    public int AttachmentId { get; set; }

    public BlogPost BlogPost { get; set; }
    public Attachment Attachment { get; set; }
}