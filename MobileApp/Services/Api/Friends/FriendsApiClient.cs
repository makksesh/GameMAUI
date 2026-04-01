using System;
using MobileApp.Models.Friends;
using MobileApp.Services.Storage;

namespace MobileApp.Services.Api.Friends;

public class FriendsApiClient : ApiClientBase, IFriendsApiClient
{
    public FriendsApiClient(IHttpClientFactory factory, ITokenStorage tokenStorage)
        : base(factory, tokenStorage) { }

    public Task<List<FriendshipDto>?> GetFriendsAsync() =>
        GetAsync<List<FriendshipDto>>("friends");

    public Task<FriendRequestDto?> SendRequestAsync(string receiverUserName) =>
        PostAsync<FriendRequestDto>($"friends/requests/{receiverUserName}");

    public Task<List<FriendRequestDto>?> GetIncomingRequestsAsync() =>
        GetAsync<List<FriendRequestDto>>("friends/requests/incoming");
    
    public Task AcceptRequestAsync(Guid requestId) =>
        PostAsync($"friends/requests/{requestId}/accept");

    public Task DeclineRequestAsync(Guid requestId) =>
        PostAsync($"friends/requests/{requestId}/decline");
    
    public Task RemoveFriendAsync(Guid friendUserId)
        => DeleteAsync($"friends/{friendUserId}");
}