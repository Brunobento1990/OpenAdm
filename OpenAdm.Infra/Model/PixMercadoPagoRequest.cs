namespace OpenAdm.Infra.Model;

public class MercadoPagoRequest
{
    public decimal Transaction_amount { get; set; }
    public string? Description { get; set; }
    public string? Notification_url { get; set; }
    public string? External_reference { get; set; }
    public string Payment_method_id { get; set; } = "pix";
    public Payer Payer { get; set; } = new();
}

public class Payer
{
    public string Email { get; set; } = string.Empty;
    public string? First_name { get; set; }
    public string? Last_name { get; set; }
    public Identification? Identification { get; set; }
}

public class Identification
{
    public string Type { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
}
