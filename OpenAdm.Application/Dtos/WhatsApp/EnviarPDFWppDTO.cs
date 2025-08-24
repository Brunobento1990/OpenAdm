using System.Text.Json.Serialization;

namespace OpenAdm.Application.Dtos.WhatsApp;

public class EnviarPDFWppDTO
{
    public string Number { get; set; } = "";

    [JsonPropertyName("mediatype")]
    public string MediaType { get; set; } = "";

    [JsonPropertyName("mimetype")]
    public string MimeType { get; set; } = "";

    public string Caption { get; set; } = "";

    public string Media { get; set; } = "";

    public string FileName { get; set; } = "";

    public int Delay { get; set; } = 0;

    public bool LinkPreview { get; set; } = false;

    public bool MentionsEveryOne { get; set; } = false;

    public List<string> Mentioned { get; set; } = new List<string>();

    public QuotedMessage? Quoted { get; set; }
}

public class QuotedMessage
{
    public KeyInfo? Key { get; set; }

    public ConversationMessage? Message { get; set; }
}

public class KeyInfo
{
    public string Id { get; set; } = "";
}

public class ConversationMessage
{
    public string Conversation { get; set; } = "";
}
