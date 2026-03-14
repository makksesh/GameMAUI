using System;
using MobileApp.Models.Skills;
using MobileApp.Services.Api.Skills;

namespace MobileApp.ViewModels.Skills;

public class SkillsViewModel : BaseViewModel
{
    private readonly ISkillsApiClient _skillsApi;

    public List<SkillDto> AllSkills        { get => _all;  set => SetProperty(ref _all,  value); }
    public List<CharacterSkillDto> MySkills { get => _mine; set => SetProperty(ref _mine, value); }

    private List<SkillDto> _all = [];
    private List<CharacterSkillDto> _mine = [];

    public Command LoadCommand { get; }
    public Command<Guid> LearnCommand { get; }
    public Command<Guid> LevelUpCommand { get; }

    public SkillsViewModel(ISkillsApiClient skillsApi)
    {
        _skillsApi = skillsApi;

        LoadCommand    = new Command(async () => await LoadAsync());
        LearnCommand   = new Command<Guid>(async id => await LearnAsync(id));
        LevelUpCommand = new Command<Guid>(async id => await LevelUpAsync(id));
    }

    public async Task LoadAsync()
    {
        await RunSafeAsync(async () =>
        {
            AllSkills = (await _skillsApi.GetAllAsync()) ?? [];
            MySkills  = (await _skillsApi.GetMyAsync())  ?? [];
        });
    }

    private async Task LearnAsync(Guid skillId)
    {
        await RunSafeAsync(async () =>
        {
            var learned = await _skillsApi.LearnAsync(skillId);
            if (learned is not null) MySkills = [..MySkills, learned];
        });
    }

    private async Task LevelUpAsync(Guid characterSkillId)
    {
        await RunSafeAsync(async () =>
        {
            var updated = await _skillsApi.LevelUpAsync(characterSkillId);
            if (updated is null) return;
            MySkills = MySkills.Select(s => s.Id == characterSkillId ? updated : s).ToList();
        });
    }
}