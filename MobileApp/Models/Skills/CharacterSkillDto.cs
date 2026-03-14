using System;

namespace MobileApp.Models.Skills;

public class CharacterSkillDto
{
    public Guid Id { get; set; }
    public Guid SkillId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    /// <summary>Active | Passive</summary>
    public string Type { get; set; } = string.Empty;
    public int CurrentLevel { get; set; }
    public int MaxLevel { get; set; }
    public decimal LevelUpCost { get; set; }
}