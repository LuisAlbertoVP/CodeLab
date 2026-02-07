namespace CodeLab.Domain.Enums;

public enum OutboundChannel
{
    Telegram = 1
}

public enum OutboundMessageStatus
{
    Pending = 1,
    Processing = 2,
    Sent = 3,
    Retry = 4,
    Failed = 5
}

public enum OutboundMessageType
{
    Text = 1
}