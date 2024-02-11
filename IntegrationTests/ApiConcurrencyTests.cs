using System.Net;
using Database;
using DatabaseIntegrationTestSample;
using Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Xunit.Extensions.AssemblyFixture;

namespace IntegrationTests;

public class ApiConcurrencyTests : IClassFixture<WebApplicationFactory<Program>>, IAssemblyFixture<MsSqlFixture>, IAsyncLifetime
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

    private class TestDbContext(DbContextOptions<SampleDbContext> options) : SampleDbContext(options)
    {
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            int changes = 0;
            var deletedAttachments = ChangeTracker.Entries<Attachment>().Where(x => x.State == EntityState.Deleted);
            foreach (var deletedAttachment in deletedAttachments)
            {
                await MessageAttachments.AddAsync(new MessageAttachment
                {
                    AttachmentId = deletedAttachment.Entity.Id,
                    Message = new Message { Body = "test" }
                }, cancellationToken);
                changes += await base.SaveChangesAsync(cancellationToken);
            }
        
            changes += await base.SaveChangesAsync(cancellationToken);
            return changes;
        }
    }
}