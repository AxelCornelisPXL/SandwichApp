namespace Kwops.Mobile.Settings;

public class DevAppSettings : IAppSettings
{
    public string OidcAuthority => "http://10.0.2.2:9000"; //Or an Azure url like "https://identity-yxe7amqvozxwe-app.azurewebsites.net";
    public string OidcClientId => "kwops.mobile";
    public string OidcClientSecret => "MobileClientSecret";
    public string OidcScope => "openid devops.read manage";
    public string OidcRedirectUri => "myapp://mauicallback";

    public string DevOpsBackendBaseUrl => "http://10.0.2.2:8000"; //Or an Azure url like "https://devops-yxe7amqvozxwe-app.azurewebsites.net";
}