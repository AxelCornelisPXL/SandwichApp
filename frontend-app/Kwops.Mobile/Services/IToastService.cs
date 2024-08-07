namespace Kwops.Mobile.Services;

public interface IToastService
{
    Task DisplayToastAsync(string message);
}