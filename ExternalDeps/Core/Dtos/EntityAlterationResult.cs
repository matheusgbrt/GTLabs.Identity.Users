using ExternalDeps.Core.Enums;

namespace ExternalDeps.Core.Dtos;

public class EntityAlterationResult<T>
{
    public bool Success { get; set; }
    public T Entity { get; set; }
    public EntityAlterationError Error { get; set; }
    public string ErrorMessage { get; set; }
}