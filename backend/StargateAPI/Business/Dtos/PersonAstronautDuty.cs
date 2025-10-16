using System.Text.Json.Serialization;

namespace StargateAPI.Business.Dtos;

// @todo: This is for handling output for the astronaut-duties endpoints.
// Prevent db schema leakage
public class PersonAstronautDuty
{
    public int Id { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Rank { get; set; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string DutyTitle { get; set; } = string.Empty;

    // @todo This should be a DateOnly
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime DutyStartDate { get; set; }

    // @todo This should be a DateOnly
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? DutyEndDate { get; set; }
}
