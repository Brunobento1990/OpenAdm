using System.Text.Json.Serialization;

namespace OpenAdm.Application.HttpClient.Response;

public class DadosCotacaoFreteResponse
{
    public ICollection<CotacaoFreteResponse> Dados { get; set; } = [];
}

public class CotacaoFreteResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Error { get; set; }
    public bool FreteValido => string.IsNullOrWhiteSpace(Error);

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal Price { get; set; }

    [JsonPropertyName("custom_price")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal CustomPrice { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal Discount { get; set; }

    [JsonPropertyName("currency")] 
    public string Currency { get; set; } = string.Empty;
    [JsonPropertyName("delivery_time")] 
    public int DeliveryTime { get; set; }
    [JsonPropertyName("delivery_range")] 
    public DeliveryRange? DeliveryRange { get; set; }

    [JsonPropertyName("custom_delivery_time")]
    public int CustomDeliveryTime { get; set; }

    [JsonPropertyName("custom_delivery_range")]
    public DeliveryRange? CustomDeliveryRange { get; set; }

    [JsonPropertyName("packages")] public ICollection<PackageInfo> Packages { get; set; } = [];

    [JsonPropertyName("additional_services")]
    public AdditionalServices? AdditionalServices { get; set; }

    [JsonPropertyName("company")] public Company Company { get; set; } = new();
}

public class DeliveryRange
{
    public int Min { get; set; }
    public int Max { get; set; }
}

public class PackageInfo
{
    [JsonPropertyName("price")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal Price { get; set; }

    [JsonPropertyName("discount")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal Discount { get; set; }

    [JsonPropertyName("format")] public string Format { get; set; } = string.Empty;

    [JsonPropertyName("dimensions")] public Dimensions Dimensions { get; set; } = new();

    [JsonPropertyName("weight")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal Weight { get; set; }

    [JsonPropertyName("insurance_value")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public decimal InsuranceValue { get; set; }

    [JsonPropertyName("products")] public ICollection<Product> Products { get; set; } = [];
}

public class Dimensions
{
    [JsonPropertyName("height")] public int Height { get; set; }

    [JsonPropertyName("width")] public int Width { get; set; }

    [JsonPropertyName("length")] public int Length { get; set; }
}

public class Product
{
    [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;

    [JsonPropertyName("quantity")] public int Quantity { get; set; }
}

public class AdditionalServices
{
    [JsonPropertyName("receipt")] public bool Receipt { get; set; }

    [JsonPropertyName("own_hand")] public bool OwnHand { get; set; }

    [JsonPropertyName("collect")] public bool Collect { get; set; }
}

public class Company
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;

    [JsonPropertyName("picture")] public string Picture { get; set; } = string.Empty;
}