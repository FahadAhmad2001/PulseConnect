using CommunityToolkit.Mvvm.Input;
using RapidResponseApplication.Models;

namespace RapidResponseApplication.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}