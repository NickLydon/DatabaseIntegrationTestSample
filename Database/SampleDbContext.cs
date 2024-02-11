using Entities;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class SampleDbContext(DbContextOptions<SampleDbContext> options) : DbContext(options)
{
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<MessageAttachment> MessageAttachments { get; set; }
    public DbSet<BlogPostAttachment> BlogPostAttachments { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<BlogPost> BlogPosts { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MessageAttachment>()
            .HasOne(x => x.Attachment);
        modelBuilder.Entity<MessageAttachment>()
            .HasOne(x => x.Message);
        modelBuilder.Entity<BlogPostAttachment>()
            .HasOne(x => x.Attachment);
        modelBuilder.Entity<BlogPostAttachment>()
            .HasOne(x => x.BlogPost);
        
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}