namespace Kwops.Mobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        Console.WriteLine("hi");
        InitializeComponent();
    }

    public async void OnMenuItemClicked(object sender, EventArgs e)
    {
        await Current.GoToAsync("//LoginPage");
    }
}