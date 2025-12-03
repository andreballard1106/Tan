using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Tandem.Application.DTOs;
using Xunit;

namespace Tandem.IntegrationTests;

public class UsersControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public UsersControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    [Fact]
    public async Task CreateUser_WithValidData_ReturnsCreated()
    {
        var request = new CreateUserRequest(
            "Matthew",
            "Decker",
            "Lund",
            "555-555-5555",
            "matt@awesomedomain.com");

        var response = await _client.PostAsJsonAsync("/api/users", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var content = await response.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<UserResponse>(content, _jsonOptions);

        user.Should().NotBeNull();
        user!.UserId.Should().NotBeEmpty();
        user.Name.Should().Be("Matthew Decker Lund");
        user.PhoneNumber.Should().Be("555-555-5555");
        user.EmailAddress.Should().Be("matt@awesomedomain.com");
    }

    [Fact]
    public async Task CreateUser_WithNullMiddleName_ReturnsCreated()
    {
        var request = new CreateUserRequest(
            "John",
            null,
            "Doe",
            "555-123-4567",
            "john@example.com");

        var response = await _client.PostAsJsonAsync("/api/users", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var content = await response.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<UserResponse>(content, _jsonOptions);

        user.Should().NotBeNull();
        user!.Name.Should().Be("John Doe");
    }

    [Fact]
    public async Task CreateUser_WithEmptyMiddleName_ReturnsCreated()
    {
        var request = new CreateUserRequest(
            "Jane",
            string.Empty,
            "Smith",
            "555-987-6543",
            "jane@example.com");

        var response = await _client.PostAsJsonAsync("/api/users", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var content = await response.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<UserResponse>(content, _jsonOptions);

        user.Should().NotBeNull();
        user!.Name.Should().Be("Jane Smith");
    }

    [Fact]
    public async Task CreateUser_WithDuplicateEmail_ReturnsConflict()
    {
        var request = new CreateUserRequest(
            "First",
            "Middle",
            "User",
            "555-111-1111",
            "duplicate@example.com");

        var firstResponse = await _client.PostAsJsonAsync("/api/users", request);
        firstResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var secondResponse = await _client.PostAsJsonAsync("/api/users", request);
        secondResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task CreateUser_WithInvalidEmail_ReturnsBadRequest()
    {
        var request = new CreateUserRequest(
            "Test",
            null,
            "User",
            "555-111-1111",
            "invalid-email");

        var response = await _client.PostAsJsonAsync("/api/users", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateUser_WithMissingFirstName_ReturnsBadRequest()
    {
        var request = new
        {
            LastName = "Doe",
            PhoneNumber = "555-111-1111",
            EmailAddress = "test@example.com"
        };

        var response = await _client.PostAsJsonAsync("/api/users", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateUser_WithMissingLastName_ReturnsBadRequest()
    {
        var request = new
        {
            FirstName = "Test",
            PhoneNumber = "555-111-1111",
            EmailAddress = "test@example.com"
        };

        var response = await _client.PostAsJsonAsync("/api/users", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateUser_WithMissingEmail_ReturnsBadRequest()
    {
        var request = new
        {
            FirstName = "Test",
            LastName = "User",
            PhoneNumber = "555-111-1111"
        };

        var response = await _client.PostAsJsonAsync("/api/users", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetUserByEmail_WithExistingUser_ReturnsUser()
    {
        var createRequest = new CreateUserRequest(
            "Matthew",
            "Decker",
            "Lund",
            "555-555-5555",
            "gettest@example.com");

        var createResponse = await _client.PostAsJsonAsync("/api/users", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var getResponse = await _client.GetAsync("/api/users?emailAddress=gettest@example.com");

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await getResponse.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<UserResponse>(content, _jsonOptions);

        user.Should().NotBeNull();
        user!.Name.Should().Be("Matthew Decker Lund");
        user.EmailAddress.Should().Be("gettest@example.com");
    }

    [Fact]
    public async Task GetUserByEmail_WithNonExistentUser_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/users?emailAddress=nonexistent@example.com");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetUserByEmail_WithMissingEmailParameter_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/users");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetUserByEmail_WithEmptyEmailParameter_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/users?emailAddress=");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateUser_WithValidData_ReturnsOk()
    {
        var createRequest = new CreateUserRequest(
            "Original",
            "Middle",
            "Name",
            "555-111-1111",
            "updatetest@example.com");

        var createResponse = await _client.PostAsJsonAsync("/api/users", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var updateRequest = new UpdateUserRequest(
            "Updated",
            "New",
            "Name",
            "555-999-9999");

        var updateResponse = await _client.PutAsJsonAsync("/api/users/updatetest@example.com", updateRequest);

        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await updateResponse.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<UserResponse>(content, _jsonOptions);

        user.Should().NotBeNull();
        user!.Name.Should().Be("Updated New Name");
        user.PhoneNumber.Should().Be("555-999-9999");
        user.EmailAddress.Should().Be("updatetest@example.com");
    }

    [Fact]
    public async Task UpdateUser_WithNonExistentUser_ReturnsNotFound()
    {
        var updateRequest = new UpdateUserRequest(
            "Test",
            null,
            "User",
            "555-111-1111");

        var response = await _client.PutAsJsonAsync("/api/users/nonexistent@example.com", updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateUser_WithInvalidData_ReturnsBadRequest()
    {
        var createRequest = new CreateUserRequest(
            "Test",
            null,
            "User",
            "555-111-1111",
            "invalidupdatetest@example.com");

        var createResponse = await _client.PostAsJsonAsync("/api/users", createRequest);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var updateRequest = new
        {
            PhoneNumber = "555-999-9999"
        };

        var updateResponse = await _client.PutAsJsonAsync("/api/users/invalidupdatetest@example.com", updateRequest);

        updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}

