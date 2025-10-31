using GTLabs.Identity.Users.Host.Consts;

namespace GTLabs.Identity.Users.Host.Dtos;

public class EntityAlterationResult<T>
{
    public bool Success { get; set; }
    public T Entity { get; set; }
    public EntityAlterationError Error { get; set; }
}