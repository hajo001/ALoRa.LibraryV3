
namespace ALoRa.Library;

public class LoRaGateway
{
    public string? Gtw_id { get; set; }
    public Int64 Timestamp { get; set; }
    public DateTime? Time { get; set; }
    public int Channel { get; set; }
    public int Rssi { get; set; }
    public decimal Snr { get; set; }
    public decimal Altitude { get; set; }
    public decimal Longitude { get; set; }
    public decimal Latitude { get; set; }
}

public class LoRaMetadataV2
{
    public DateTime? Time { get; set; }
    public decimal Frequency { get; set; }
    public string? Modulation { get; set; }
    public string? Data_rate { get; set; }
    public string? Coding_rate { get; set; }

    public LoRaGateway[]? Gateways { get; set; }

    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public decimal Altitude { get; set; }
    public string? Location_source { get; set; }
}

public class LoRaMessageV2
{
    public string? App_id { get; set; }
    public string? Dev_id { get; set; }
    public string? Hardware_serial { get; set; }
    public int Port { get; set; }
    public int Counter { get; set; }
    public string? Payload_raw { get; set; }
    public LoRaMetadataV2? Metadata { get; set; }
}
