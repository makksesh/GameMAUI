using System;
using MobileApp.Models.Skills;

namespace MobileApp.Services.Api.Skills;

public interface ISkillsApiClient
{
    Task<List<SkillDto>?> GetAllAsync();
    Task<List<CharacterSkillDto>?> GetMyAsync();
    Task<CharacterSkillDto?> LearnAsync(Guid skillId);
    Task<CharacterSkillDto?> LevelUpAsync(Guid characterSkillId);
}