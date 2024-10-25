namespace OpenAdm.Infra.Model;

public class MercadoPagoResponse
{
    public long Id { get; set; }
    public DateTime Date_created { get; set; }
    public object? Date_approved { get; set; }
    public DateTime Date_last_updated { get; set; }
    public DateTime Date_of_expiration { get; set; }
    public object? Money_release_date { get; set; }
    public string Money_release_status { get; set; } = string.Empty;
    public string Operation_type { get; set; } = string.Empty;
    public string Issuer_id { get; set; } = string.Empty;
    public string Payment_method_id { get; set; } = string.Empty;
    public string Payment_type_id { get; set; } = string.Empty;
    public PaymentMethod? Payment_method { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Status_detail { get; set; } = string.Empty;
    public string Currency_id { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool Live_mode { get; set; }
    public object? Sponsor_id { get; set; }
    public object? Authorization_code { get; set; }
    public object? Money_release_schema { get; set; }
    public int Taxes_amount { get; set; }
    public object? Counter_currency { get; set; }
    public object? Brand_id { get; set; }
    public int Shipping_amount { get; set; }
    public string Build_version { get; set; } = string.Empty;
    public object? Pos_id { get; set; }
    public object? Store_id { get; set; }
    public object? Integrator_id { get; set; }
    public object? Platform_id { get; set; }
    public object? Corporation_id { get; set; }
    public Payer? Payer { get; set; }
    public int Collector_id { get; set; }
    public object? Marketplace_owner { get; set; }
    public Metadata? Metadata { get; set; }
    public AdditionalInfo? Additional_info { get; set; }
    public Order? Order { get; set; }
    public object? External_reference { get; set; }
    public int Coupon_amount { get; set; }
    public object? Differential_pricing_id { get; set; }
    public object? Financing_group { get; set; }
    public object? Deduction_schema { get; set; }
    public object? Callback_url { get; set; }
    public int Installments { get; set; }
    public TransactionDetails? Transaction_details { get; set; }
    public List<object>? Fee_details { get; set; }
    public List<ChargesDetail>? Charges_details { get; set; }
    public bool Captured { get; set; }
    public bool Binary_mode { get; set; }
    public object? Call_for_authorize_id { get; set; }
    public object? Statement_descriptor { get; set; }
    public Card? Card { get; set; }
    public object? Notification_url { get; set; }
    public List<object>? Refunds { get; set; }
    public string Processing_mode { get; set; } = string.Empty;
    public object? Merchant_account_id { get; set; }
    public object? Merchant_number { get; set; }
    public List<object>? Acquirer_reconciliation { get; set; }
    public PointOfInteraction? Point_of_interaction { get; set; }
    public object? Accounts_info { get; set; }
    public object? Tags { get; set; }
}

public class AdditionalInfo
{
    public object? Available_balance { get; set; }
    public object? Nsu_processadora { get; set; }
    public object? Authentication_code { get; set; }
}

public class Metadata
{
}

public class Order
{
}

public class ChargesDetail
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public Accounts? Accounts { get; set; }
    public int Client_id { get; set; }
    public DateTime Date_created { get; set; }
    public DateTime Last_updated { get; set; }
    public Amounts? Amounts { get; set; }
    public Metadata? Metadata { get; set; }
    public object? Reserve_id { get; set; }
    public List<object>? Refund_charges { get; set; }
}

public class Amounts
{
    public double Original { get; set; }
    public int Refunded { get; set; }
}

public class Accounts
{
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
}

public class Card
{
}

public class BusinessInfo
{
    public string Unit { get; set; } = string.Empty;
    public string Sub_unit { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
}

public class Location
{
    public object? State_id { get; set; }
    public object? Source { get; set; }
}

public class BankInfo
{
    public Payer? Payer { get; set; }
    public Collector? Collector { get; set; }
    public object? Is_same_bank_account_owner { get; set; }
    public object? Origin_bank_id { get; set; }
    public object? Origin_wallet_id { get; set; }
}

public class Collector
{
    public object? Account_id { get; set; }
    public object? Long_name { get; set; }
    public string Account_holder_name { get; set; } = string.Empty;
    public object? Transfer_account_id { get; set; }
}

public class TransactionData
{
    public string Qr_code { get; set; } = string.Empty;
    public object? Bank_transfer_id { get; set; }
    public object? Transaction_id { get; set; }
    public object? E2e_id { get; set; }
    public object? Financial_institution { get; set; }
    public string Ticket_url { get; set; } = string.Empty;
    public BankInfo? Bank_info { get; set; }
    public string Qr_code_base64 { get; set; } = string.Empty;
}

public class PointOfInteraction
{
    public string Type { get; set; } = string.Empty;
    public BusinessInfo? Business_info { get; set; }
    public Location? Location { get; set; }
    public ApplicationData? Ppplication_data { get; set; }
    public TransactionData? Transaction_data { get; set; }
}

public class ApplicationData
{
    public object? Name { get; set; }
    public object? Version { get; set; }
}

public class PaymentMethod
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Issuer_id { get; set; } = string.Empty;
}

public class TransactionDetails
{
    public object? Payment_method_reference_id { get; set; }
    public object? Acquirer_reference { get; set; }
    public int Net_received_amount { get; set; }
    public int Overpaid_amount { get; set; }
    public object? External_resource_url { get; set; }
    public int Installment_amount { get; set; }
    public object? Financial_institution { get; set; }
    public object? Payable_deferral_period { get; set; }
    public object? Bank_transfer_id { get; set; }
    public object? Transaction_id { get; set; }
}
