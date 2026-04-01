using System;
using MobileApp.Models.Friends;

namespace MobileApp.Services.Api.Friends;

public interface IFriendsApiClient
{
    Task<List<FriendshipDto>?> GetFriendsAsync();
    Task<FriendRequestDto?> SendRequestAsync(string receiverId);
    Task<List<FriendRequestDto>?> GetIncomingRequestsAsync();
    Task AcceptRequestAsync(Guid requestId);
    Task DeclineRequestAsync(Guid requestId);
    Task RemoveFriendAsync(Guid friendUserId);
}