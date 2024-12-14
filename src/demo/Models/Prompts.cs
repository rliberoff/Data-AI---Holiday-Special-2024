using System.ComponentModel.DataAnnotations;

namespace Demo.Models;

internal sealed class Prompts
{
    [Required(AllowEmptyStrings = false)]
    public required string Mistral { get; init; }

    [Required(AllowEmptyStrings = false)]
    public required string Phi3 { get; init; }

    [Required(AllowEmptyStrings = false)]
    public required string Llama { get; init; }
}
