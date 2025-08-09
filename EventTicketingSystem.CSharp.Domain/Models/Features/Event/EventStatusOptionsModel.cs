namespace EventTicketingSystem.CSharp.Domain.Models.Features.Event;

public class EventStatusOptionsModel
{
    public List<OptionItem> EventStatusOptions { get; set; }
}

public class OptionItem
{
    public string Value { get; set; } = null!;

    public string Label { get; set; } = null!;

}