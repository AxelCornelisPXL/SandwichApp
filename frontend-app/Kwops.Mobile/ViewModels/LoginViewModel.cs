using System.Windows.Input;
using Kwops.Mobile.Services;
using Kwops.Mobile.Services.Identity;

namespace Kwops.Mobile.ViewModels;

public class LoginViewModel : BaseViewModel
{
    public ICommand LoginCommand { private set; get; }

    public LoginViewModel(IIdentityService iis, ITokenProvider itp, INavigationService ins, IToastService its)
    {
        LoginCommand = new Command(
            execute: async () =>
            {
                RefreshCanExecutes(true);
                var login = await iis.LoginAsync();
                if (login.IsError)
                {
                    its.DisplayToastAsync(login.ErrorDescription);
                    RefreshCanExecutes(false);
                }
                else
                {
                    itp.AuthAccessToken = login.AccessToken;
                    ins.NavigateAsync("TeamsPage");
                    RefreshCanExecutes(false);
                }
            },
            canExecute: () => !IsBusy);
    }

    private void RefreshCanExecutes(bool isBusy)
    {
        IsBusy = isBusy;
        ((Command)LoginCommand).ChangeCanExecute();
    }
}