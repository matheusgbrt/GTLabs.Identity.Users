namespace GTLabs.Identity.Users.Host.Dtos;

public class EntitySearchResult<T>
{
    public T Entity { get; set; }
    public bool Found { get; set; }
}