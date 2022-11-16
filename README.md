# ALoRa.LibraryV3
For TTN V3

A simple console app can be created as follows

```C#
class Program
{
    private static bool CONTAINER = false;
    static void Main(string[] args)
    {
        Console.WriteLine("\nALoRa ConsoleApp - A The Things Network C# Library\n");

        using (var app = new TTNApplication(TTN_APP_ID, TTN_ACCESS_KEY, TTN_REGION))
        {
            app.MessageReceived += App_MessageReceived;

            if (CONTAINER)
            {
                // use for testing when running as container
                Thread.Sleep(Timeout.Infinite);
            }
            else
            {
                Console.WriteLine("Press return to exit!");
                Console.ReadLine();
                Console.WriteLine("\nAloha, Goodbye, Vaarwel!");
                Thread.Sleep(1000);
            }
            app.Dispose();
        }
    }

    private static void App_MessageReceived(TTNMessage obj)
    {
        var data = obj.Payload != null ? BitConverter.ToString(obj.Payload) : string.Empty;
        Console.WriteLine($"Message Timestamp: {obj.Timestamp}, Device: {obj.DeviceID}, Topic: {obj.Topic}, Payload: {data}");
    }

    /// <summary>
    /// Use this method for App.config files outside the app folder: https://stackoverflow.com/questions/10656077/what-is-wrong-with-my-app-config-file
    /// </summary>
    /// <param name="appSettingKey"></param>
    /// <returns>Appsetting value</returns>
    public static string GetAppSettingValue(string appSettingKey)
    {
        try
        {
            ExeConfigurationFileMap fileMap = new();
            fileMap.ExeConfigFilename = "/vm/conf/App.config";

            var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            var value = configuration.AppSettings.Settings[appSettingKey].Value;

            //var value = ConfigurationManager.AppSettings[appSettingKey];
            if (string.IsNullOrEmpty(value))
            {
                var message = $"Cannot find value for appSetting key: '{appSettingKey}'.";
                throw new ConfigurationErrorsException(message);
            }
            return value;
        }
        catch (Exception e)
        {
            Console.WriteLine($"The appSettingKey: {appSettingKey} could not be read!");
            Console.WriteLine($"Exception: {e.Message}");
            return "";
        }
    }


    /// <summary>
    ///  /// Use this method for App.config files outside the app folder: https://stackoverflow.com/questions/10656077/what-is-wrong-with-my-app-config-file
    /// </summary>
    /// <param name="connectionStringKey"></param>
    /// <returns>connectionString value</returns>
    public static string GetConnectionStringValue(string connectionStringKey)
    {
        try
        {
            ExeConfigurationFileMap fileMap = new();
            fileMap.ExeConfigFilename = "/vm/conf/App.config";

            var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            var value = configuration.ConnectionStrings.ConnectionStrings[connectionStringKey].ConnectionString;

            //var value = ConfigurationManager.ConnectionStrings[connectionStringKey].ToString();
            if (string.IsNullOrEmpty(value))
            {
                var message = $"Cannot find value for connectionString key: '{connectionStringKey}'.";
                throw new ConfigurationErrorsException(message);
            }
            return value;
        }
        catch (Exception e)
        {
            Console.WriteLine($"The connectionStringKey: {connectionStringKey} could not be read!");
            Console.WriteLine($"Exception: {e.Message}");
            return "";
        }
    }
}
```
