#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Common.Configuration;

public record RabbitMQConfiguration
{
    [Required]
    public string Hostname { get; init; }
    
    [Required]
    public int Port { get; init; }
    
    [Required]
    public string UserName { get; init; }
    
    [Required]
    public string Password { get; init; }
}