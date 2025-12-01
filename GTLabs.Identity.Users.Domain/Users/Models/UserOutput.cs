using GTLabs.Identity.Users.Domain.Users.Entities;

namespace GTLabs.Identity.Users.Domain.Users.Models;

public class UserOutput
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime? CreationTime { get; set; }
    public DateTime? LastModificationTime { get; set; }
    public Guid? CreatorId { get; set; }
    public Guid? ModifierId { get; set; }
}