using System;

namespace MobileApp.Models;

public class CharacterDto
{
    public Guid Id { get; set; }
    public 
        Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public int Experience { get; set; }
    public int Health { get; set; }
    public int Mana { get; set; }
    public int Armor { get; set; }
    public int Damage { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; }
}