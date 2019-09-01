using Microsoft.WindowsAzure.MobileServices;

public static class AzureMobileServiceClient 
{

    private const string backendUrl = "https://unity-tables.azurewebsites.net/";
    private static MobileServiceClient client;

    public static MobileServiceClient Client
    {
        get
        {
            if (client == null)
            {
              client = new MobileServiceClient(backendUrl);
            }
            return client;
        }
        
    }
}
