using Testcontainers.MsSql;

namespace IntegrationTests;

public class MsSqlFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder().Build();

    public string GetConnectionString(Guid database) => _container.GetConnectionString().Replace("Database=master", $"Database={database}");

    public Task InitializeAsync() => _container.StartAsync();

    public Task DisposeAsync() => _container.StopAsync();
}