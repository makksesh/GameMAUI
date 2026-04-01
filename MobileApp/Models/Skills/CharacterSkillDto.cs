using System;

namespace MobileApp.Models.Skills;

public class CharacterSkillDto
{
    public Guid Id { get; set; }
    public Guid SkillId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int CurrentLevel { get; set; }
    public int MaxLevel { get; set; }
    public decimal LevelUpCost { get; set; }

    /// <summary>Прогресс для ProgressBar от 0.0 до 1.0. Не сериализуется.</summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public double LevelProgress =>
        MaxLevel > 0 ? Math.Clamp((double)CurrentLevel / MaxLevel, 0.0, 1.0) : 0.0;
}
