using System;
using MobileApp.Models.Skills;
using MobileApp.Services.Api.Skills;

namespace MobileApp.ViewModels.Skills;

public class SkillsViewModel : BaseViewModel
{
    private readonly ISkillsApiClient skillsApi;

    private List<SkillDto> allSkills = [];
    public List<SkillDto> AllSkills
    {
        get => allSkills;
        set
        {
            if (SetProperty(ref allSkills, value))
                RebuildGroups();
        }
    }

    private List<CharacterSkillDto> mySkills = [];
    public List<CharacterSkillDto> MySkills
    {
        get => mySkills;
        set
        {
            if (SetProperty(ref mySkills, value))
                RebuildGroups();
        }
    }

    private List<SkillDto> unlearnedSkills = [];
    public List<SkillDto> UnlearnedSkills
    {
        get => unlearnedSkills;
        set => SetProperty(ref unlearnedSkills, value);
    }

    public bool HasUnlearnedSkills => UnlearnedSkills.Any();
    public bool HasLearnedSkills => MySkills.Any();

    public Command LoadCommand { get; }
    public Command<Guid> LearnCommand { get; }
    public Command<Guid> LevelUpCommand { get; }

    public SkillsViewModel(ISkillsApiClient skillsApi)
    {
        this.skillsApi = skillsApi;

        LoadCommand = new Command(async () => await LoadAsync());
        LearnCommand = new Command<Guid>(async id => await LearnAsync(id));
        LevelUpCommand = new Command<Guid>(async id => await LevelUpAsync(id));
    }

    public async Task LoadAsync()
    {
        await RunSafeAsync(async () =>
        {
            AllSkills = await skillsApi.GetAllAsync() ?? [];
            MySkills = await skillsApi.GetMyAsync() ?? [];
            RebuildGroups();
        });
    }

    private void RebuildGroups()
    {
        var learnedSkillIds = MySkills
            .Select(x => x.SkillId)
            .ToHashSet();

        UnlearnedSkills = AllSkills
            .Where(x => !learnedSkillIds.Contains(x.Id))
            .ToList();

        OnPropertyChanged(nameof(HasUnlearnedSkills));
        OnPropertyChanged(nameof(HasLearnedSkills));
    }

    private async Task LearnAsync(Guid skillId)
    {
        await RunSafeAsync(async () =>
        {
            var learned = await skillsApi.LearnAsync(skillId);
            if (learned is null)
                return;

            MySkills = [.. MySkills, learned];
            RebuildGroups();
        });
    }

    private async Task LevelUpAsync(Guid characterSkillId)
    {
        await RunSafeAsync(async () =>
        {
            var updated = await skillsApi.LevelUpAsync(characterSkillId);
            if (updated is null)
                return;

            MySkills = MySkills
                .Select(s => s.Id == characterSkillId ? updated : s)
                .ToList();

            RebuildGroups();
        });
    }
}
