namespace LifeOrganizer.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entityName, object key)
        : base($"Could not find \"{entityName}\" with ID: {key}")
    {
    }
}
