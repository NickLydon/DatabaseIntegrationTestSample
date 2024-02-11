using System.Net;
using Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Xunit.Extensions.AssemblyFixture;

namespace IntegrationTests;

public class ApiTests : IClassFixture<WebApplicationFactory<Program>>, IAssemblyFixture<MsSqlFixture>, IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory;

    public ApiTests(WebApplicationFactory<Program> factory, MsSqlFixture msSqlFixture)
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
        });
    }

    [Fact]
    public async Task GivenAttachmentIsInUseShouldNotBeAbleToDeleteIt()
    {
        var client = _factory.CreateClient();
        
        var createAttachmentResponse = await client.PostAsync("attachment?url=example", null);
        createAttachmentResponse.EnsureSuccessStatusCode();
        var attachment = await createAttachmentResponse.Content.ReadFromJsonAsync<CreateAttachmentResponse>();

        var createMessageResponse = await client.PostAsJsonAsync("message", new
        {
            body = "message body",
            attachmentIds = new[] { attachment.Id }
        });
        createMessageResponse.EnsureSuccessStatusCode();
        
        var deleteAttachmentResponse = await client.DeleteAsync($"attachment?id={attachment.Id}");
        Assert.Equal(HttpStatusCode.BadRequest, deleteAttachmentResponse.StatusCode);
    }

    // ReSharper disable once ClassNeverInstantiated.Local
    private record CreateAttachmentResponse(int Id);
    
    public async Task InitializeAsync()
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SampleDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _factory.DisposeAsync();
    }
}