using Kwops.Mobile.ViewModels;

namespace Kwops.Mobile.Views;

public partial class TeamsPage : ContentPage
{
    private readonly TeamsViewModel _viewModel;
    public TeamsPage(TeamsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing();

    }
}