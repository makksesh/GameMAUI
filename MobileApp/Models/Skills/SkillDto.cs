using System;

namespace MobileApp.Models.Skills;

public class SkillDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    /// <summary>Active | Passive</summary>
    public string Type { get; set; } = string.Empty;
    public int MaxLevel { get; set; }
    public decimal LevelUpCost { get; set; }
}