using System;
using MobileApp.Models;

namespace MobileApp.Services.Api.Character;

public interface ICharacterApiClient
{
    Task<CharacterDto?> CreateAsync(CreateCharacterRequest request);
    Task<CharacterDto?> GetMyAsync();
    Task<CharacterDto?> GetByIdAsync(Guid id);
    Task<CharacterDto?> UpgradeStatAsync(UpgradeStatRequest request);
}