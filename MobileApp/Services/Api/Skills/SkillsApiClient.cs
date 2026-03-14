using System;
using MobileApp.Models.Skills;
using MobileApp.Services.Storage;

namespace MobileApp.Services.Api.Skills;

public class SkillsApiClient : ApiClientBase, ISkillsApiClient
{
    public SkillsApiClient(IHttpClientFactory factory, ITokenStorage tokenStorage)
        : base(factory, tokenStorage) { }

    public Task<List<SkillDto>?> GetAllAsync() =>
        GetAsync<List<SkillDto>>("skills");

    public Task<List<CharacterSkillDto>?> GetMyAsync() =>
        GetAsync<List<CharacterSkillDto>>("skills/my");

    public Task<CharacterSkillDto?> LearnAsync(Guid skillId) =>
        PostAsync<CharacterSkillDto>($"skills/{skillId}/learn");

    public Task<CharacterSkillDto?> LevelUpAsync(Guid characterSkillId) =>
        PostAsync<CharacterSkillDto>($"skills/my/{characterSkillId}/levelup");
}