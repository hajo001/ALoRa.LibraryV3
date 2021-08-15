# ALoRa.LibraryV3
For TTN V3

A simple console app can be created as follows

```C#
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("\nALoRa ConsoleApp - A The Things Network C# Library\n");

        var app = new TTNApplication("TTN_APP_ID", "TTN_ACCESS_KEY", "TTN_REGION");
        app.MessageReceived += App_MessageReceived;

        Console.WriteLine("Press return to exit!");
        Console.ReadLine();

        app.Dispose();

        Console.WriteLine("\nAloha, Goodbye, Vaarwel!");

        System.Threading.Thread.Sleep(1000);
    }

    private static void App_MessageReceived(TTNMessage obj)
    {
        var data = obj.Payload != null ? BitConverter.ToString(obj.Payload) : string.Empty;
        Console.WriteLine($"Message Timestamp: {obj.Timestamp}, Device: {obj.DeviceID}, Topic: {obj.Topic}, Payload: {data}");
    }
}
```
