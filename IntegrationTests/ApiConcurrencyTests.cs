using System.Net;
using Database;
using DatabaseIntegrationTestSample;
using Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Xunit.Extensions.AssemblyFixture;

namespace IntegrationTests;

public class ApiConcurrencyTests : IClassFixture<WebApplicationFactory<Program>>, IAssemblyFixture<MsSqlFixture>, IAsyncLifetime, ICommitInterceptor
{
    private readonly WebApplicationFactory<Program> _factory;

    public ApiConcurrencyTests(WebApplicationFactory<Program> factory, MsSqlFixture msSqlFixture)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, configurationBuilder) =>
            {
                configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>()
                {
                    ["ConnectionStrings:SampleDb"] = msSqlFixture.GetConnectionString(Guid.NewGuid())
                });
            });
            builder.ConfigureTestServices(collection =>
            {
                collection.BuildServiceProvider().GetRequiredService<SampleDbContext>().Database.Migrate();
                collection.Remove(collection.Single(x => x.ImplementationType == typeof(SampleDbContext)));
                collection.AddSampleDbContext<TestDbContext>();
                collection.AddSingleton<ICommitInterceptor>(this);
            });
        });
    }

    [Fact]
    public async Task GivenAttachmentIsInUseShouldNotBeAbleToDeleteIt()
    {
        var client = _factory.CreateClient();
        
        var createAttachmentResponse = await client.PostAsync("attachment?url=example", null);
        createAttachmentResponse.EnsureSuccessStatusCode();
        var attachment = await createAttachmentResponse.Content.ReadFromJsonAsync<CreateAttachmentResponse>();

        var deleteAttachmentResponse = await client.DeleteAsync($"attachment?id={attachment.Id}");
        Assert.Equal(HttpStatusCode.BadRequest, deleteAttachmentResponse.StatusCode);
    }

    // ReSharper disable once ClassNeverInstantiated.Local

    private record CreateAttachmentResponse(int Id);

    public async Task InitializeAsync()
    {
    }

    public async Task DisposeAsync()
    {
        await _factory.DisposeAsync();
    }

    private class TestDbContext(DbContextOptions<SampleDbContext> options, ICommitInterceptor commitInterceptor) : SampleDbContext(options)
    {
        public Task<int> TestSaveChangesAsync(CancellationToken cancellationToken = new()) => base.SaveChangesAsync(cancellationToken);

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            var deletedAttachments = ChangeTracker.Entries<Attachment>()
                .Where(x => x.State == EntityState.Deleted)
                .Select(x => x.Entity);
            await commitInterceptor.OnAttachmentsDeleted(deletedAttachments);
            
            return await base.SaveChangesAsync(cancellationToken);
        }
    }

    async Task ICommitInterceptor.OnAttachmentsDeleted(IEnumerable<Attachment> deletedAttachments)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TestDbContext>();
        foreach (var deletedAttachment in deletedAttachments)
        {
            await dbContext.MessageAttachments.AddAsync(new MessageAttachment
            {
                AttachmentId = deletedAttachment.Id,
                Message = new Message { Body = "test" }
            });
        }

        await dbContext.TestSaveChangesAsync();
    }
}

internal interface ICommitInterceptor
{
    Task OnAttachmentsDeleted(IEnumerable<Attachment> deletedAttachments);
}