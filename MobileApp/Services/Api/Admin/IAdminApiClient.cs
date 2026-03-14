using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MobileApp.Models.Admin;

namespace MobileApp.Services.Api.Admin;

public interface IAdminApiClient
{
    Task<List<UserDto>?> GetAllUsersAsync();
    Task<UserDto?> BlockUserAsync(Guid userId);
    Task<UserDto?> UnblockUserAsync(Guid userId);
    Task<UserDto?> ChangeRoleAsync(Guid userId, UpdateRoleRequest request);
}