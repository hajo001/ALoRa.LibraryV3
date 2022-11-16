
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

// https://github.com/tomakita/Colorful.Console
using Console = Colorful.Console;
using System.Drawing;

namespace ALoRa.Library;

public class TTNApplication : BaseObject
{
    private const string BROKER_URL_FILTER = "{0}.cloud.thethings.network";
    private const string TOPIC_ALL_DEVICES_UP = "v3/+/devices/+/up";
    //private const string TOPIC_ALL_DEVICES_UP = "#";
    private const int NETWORK_SLEEP_TIME = 30*1000; // 30 sec.
    private const bool USE_SECURE_CONNECTION = true;

    private readonly MqttClient m_client;
    private Action<TTNMessage>? m_msgReceived;
    private bool _disposed = false;

    public event Action<TTNMessage> MessageReceived
    {
        add { m_msgReceived += value; }
        remove { m_msgReceived -= value; }
    }

    private string AppId { get; }
    private string ClientId { get; }
    private string AccessKey { get; }

    public bool IsConnected
    {
        get { return m_client.IsConnected; }
    }

    private void TryToReconnect()
    {
        while(!IsConnected)
        {
            Thread.Sleep(NETWORK_SLEEP_TIME);
            try
            {
                ConnectAndSubsribe();
            }
            catch
            {
                Console.WriteLine($"{Environment.NewLine}Network is still DOWN!", Color.Red);
            }
        }
        Console.WriteLine($"{Environment.NewLine}Network is UP!", Color.Green);
    }

    public void ConnectAndSubsribe()
    {
        m_client.Connect(ClientId, AppId, AccessKey);
        m_client.Subscribe(new string[] { TOPIC_ALL_DEVICES_UP }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
    }

    public TTNApplication(string appId, string accessKey, string region)
    {
        AppId = appId;
        ClientId = Guid.NewGuid().ToString();
        AccessKey = accessKey;
        try
        {
            string brokerHostName = string.Format(BROKER_URL_FILTER, region);
            // https://gist.github.com/KeesCBakker/d8e093c237bf939004f830406cb35c09
            if (USE_SECURE_CONNECTION)
                m_client = new MqttClient(brokerHostName, 8883, true, null, null, MqttSslProtocols.TLSv1_2);
            else
                m_client = new MqttClient(brokerHostName, 1883, false, null, null, MqttSslProtocols.None);

            m_client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            m_client.ConnectionClosed += Client_MqttConnectionClosed;
            m_client.MqttMsgSubscribed += Client_MqttMsgSubscribed;
            m_client.MqttMsgUnsubscribed += Client_MqttMsgUnsubscribed;

            ConnectAndSubsribe();
        }
        catch (Exception ex) {
            Console.WriteLine($"{Environment.NewLine}Exception {ex.ToString()}", Color.Red);
            Environment.Exit(1);
        }
    }

    protected override void Dispose(bool disposing)
    {
        m_client.Disconnect();
        _disposed = true;
    }

    /// <summary>
    // public ushort Publish(string topic, byte[] message);
    // public ushort Publish(string topic, byte[] message, byte qosLevel, bool retain);
    /// </summary>
    public void Publish(string publish, byte[] message)
    {
        Console.WriteLine($"Published: {publish}");
        m_client.Publish(publish, message);
    }

    private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        try
        {
            var msg = TTNMessage.DeserialiseMessageV3(e);
            m_msgReceived?.Invoke(msg);
        }
        catch (Exception ex)
        {
            // Swallow any exceptions during message receive, but indicate errors
            Console.WriteLine($"{Environment.NewLine}Exception {ex.ToString()}", Color.Red);
        }
    }

    private void Client_MqttMsgUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
    {
        Console.WriteLine($"{Environment.NewLine}Client_MqttMsgUnsubscribed: {sender.ToString()}", Color.Yellow);
    }

    private void Client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
    {
        Console.WriteLine($"{Environment.NewLine}Client_MqttMsgSubscribed: {sender.ToString()} to topic: {TOPIC_ALL_DEVICES_UP}", Color.Yellow);
    }

    private void Client_MqttConnectionClosed(object sender, EventArgs e)
    {
        Console.WriteLine($"{Environment.NewLine}Client_MqttConnectionClosed: {sender.ToString()}", Color.Red);
        if (!_disposed)
        {
            Console.WriteLine($"{Environment.NewLine}Trying To Reconnect!", Color.Blue);
            TryToReconnect();
        }
    }
}
