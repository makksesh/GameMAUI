using System;
using MobileApp.Models;
using MobileApp.Services.Storage;

namespace MobileApp.Services.Api.Character;

public class CharacterApiClient : ApiClientBase, ICharacterApiClient
{
    public CharacterApiClient(IHttpClientFactory factory, ITokenStorage tokenStorage)
        : base(factory, tokenStorage) { }

    public Task<CharacterDto?> CreateAsync(CreateCharacterRequest request) =>
        PostAsync<CharacterDto>("characters", request);

    public Task<CharacterDto?> GetMyAsync() =>
        GetAsync<CharacterDto>("characters/me");

    public Task<CharacterDto?> GetByIdAsync(Guid id) =>
        GetAsync<CharacterDto>($"characters/{id}");

    public Task<CharacterDto?> UpgradeStatAsync(UpgradeStatRequest request) =>
        PatchAsync<CharacterDto>("characters/me/stats", request);
}