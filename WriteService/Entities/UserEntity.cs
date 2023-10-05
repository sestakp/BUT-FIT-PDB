using System.ComponentModel.DataAnnotations;

namespace WriteService.Entities
{
    public class UserEntity
    {
        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
