using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MobileApp.Models.Admin;
using MobileApp.Services.Storage;

namespace MobileApp.Services.Api.Admin;

public class AdminApiClient : ApiClientBase, IAdminApiClient
{
    public AdminApiClient(IHttpClientFactory factory, ITokenStorage tokenStorage)
        : base(factory, tokenStorage) { }

    public Task<List<UserDto>?> GetAllUsersAsync() =>
        GetAsync<List<UserDto>>("admin/users");

    public Task<UserDto?> BlockUserAsync(Guid userId) =>
        PostAsync<UserDto>($"admin/users/{userId}/block");

    public Task<UserDto?> UnblockUserAsync(Guid userId) =>
        PostAsync<UserDto>($"admin/users/{userId}/unblock");

    public Task<UserDto?> ChangeRoleAsync(Guid userId, UpdateRoleRequest request) =>
        PatchAsync<UserDto>($"admin/users/{userId}/role", request);
}