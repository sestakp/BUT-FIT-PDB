#nullable disable

using System.ComponentModel.DataAnnotations;

namespace ReadService.Data;

public record MongoDbConfiguration
{
    [Required]
    public string DatabaseName { get; init; }
    
    [Required]
    public string ConnectionString { get; init; }
}