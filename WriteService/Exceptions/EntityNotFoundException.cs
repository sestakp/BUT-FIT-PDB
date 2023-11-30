namespace WriteService.Exceptions;

public sealed class EntityNotFoundException : Exception
{
    public EntityNotFoundException(long id) : base($"Entity with id '{id}' was not found.")
    {
    }

    public EntityNotFoundException(string id) : base($"Entity with id '{id}' was not found.")
    {
    }
}