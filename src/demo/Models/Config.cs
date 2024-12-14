using System.ComponentModel.DataAnnotations;

namespace Demo.Models;

internal sealed class Config
{
    [Required(AllowEmptyStrings = false)]
    public required string DeploymentName { get; init; }

    [Required(AllowEmptyStrings = false)]
    public required string ApiKey { get; init; }

    [Required(AllowEmptyStrings = false)]
    public required string Endpoint { get; init; }

    [Required(AllowEmptyStrings = false)]
    public required string ModelId { get; init; }

    [Required(AllowEmptyStrings = false)]
    public required string ServiceId { get; init; }
}
