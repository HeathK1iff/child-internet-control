public record KeeneticOptions
{
    public const string SectionName = "Router";
    public string Host { get; set; } = string.Empty;
    public Credential Credential { get; set; } = new();
}

public record Credential
{
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
} 

