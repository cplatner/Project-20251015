using System.Text.Json.Serialization;

namespace StargateAPI.Business.Dtos;

public class PersonAstronaut
{
    public int PersonId { get; init; }

    public string Name { get; init; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CurrentRank { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CurrentDutyTitle { get; init; }

    // @todo This should be a DateOnly
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? CareerStartDate { get; init; }

    // @todo This should be a DateOnly
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? CareerEndDate { get; init; }
}