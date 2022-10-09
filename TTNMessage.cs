using Newtonsoft.Json;
using System.Text;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ALoRa.Library;

public class TTNMessage
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="evt"></param>
    /// <returns></returns>
    public static TTNMessage DeserialiseMessageV2(MqttMsgPublishEventArgs evt)
    {
        var text = Encoding.ASCII.GetString(evt.Message);
        var lora = JsonConvert.DeserializeObject<LoRaMessageV2>(text);
        var msg = new TTNMessage(lora!, evt.Topic);
        return msg;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="evt"></param>
    /// <returns></returns>
    public static TTNMessage DeserialiseMessageV3(MqttMsgPublishEventArgs evt)
    {
        var text = Encoding.ASCII.GetString(evt.Message);
        var lora = JsonConvert.DeserializeObject<LoRaMessageV3>(text);
        var msg = new TTNMessage(lora!, evt.Topic);
        return msg;
    }

    public DateTime? Timestamp { get; set; }
    public string? DeviceID { get; set; }
    public LoRaMessageV2? RawMessageV2 { get; set; }
    public LoRaMessageV3? RawMessageV3 { get; set; }
    public string? Topic { get; set; }
    public byte[]? Payload { get; set; }

    // for virtual sensor
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public string? Altitude { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="topic"></param>
    /// <returns></returns>
    public TTNMessage(LoRaMessageV2 msg, string topic)
    {
        if (msg == null) return;

        Timestamp = msg.Metadata!.Time;
        DeviceID = msg.Dev_id!;
        RawMessageV2 = msg;
        Topic = topic;

        if (msg.Payload_raw != null)
        {
            Payload = Convert.FromBase64String(msg.Payload_raw);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="topic"></param>
    /// <returns></returns>
    public TTNMessage(LoRaMessageV3 msg, string topic)
    {
        Timestamp = msg.uplink_message!.settings!.time;
        DeviceID = msg.end_device_ids!.device_id;
        RawMessageV3 = msg;
        Topic = topic;

        if (msg.uplink_message.frm_payload != null)
        {
            Payload = Convert.FromBase64String(msg.uplink_message.frm_payload);
        }
    }
}
