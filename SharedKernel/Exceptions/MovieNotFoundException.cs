namespace KinoPoshuk.SharedKernel.Exceptions;

public class MovieNotFoundException : Exception
{
    public MovieNotFoundException(string title) : base($"Фільм \"{title}\" не знайдено")
    {
    }
}
