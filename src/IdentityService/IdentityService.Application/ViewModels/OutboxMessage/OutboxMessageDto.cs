namespace IdentityService.Application.ViewModels.OutboxMessage;

public class OutboxMessageDto
{
    public string Data { get; set; }
    public string Id { get; set; }

    public string Type { get; set; }
}