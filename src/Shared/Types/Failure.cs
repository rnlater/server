using Shared.Constants;

namespace Shared.Types;

public class Failure
{
    public List<ErrorMessage> ErrorMessages { get; }

    public Failure(List<ErrorMessage> errorMessages)
    {
        ErrorMessages = errorMessages;
    }

    public Failure(ErrorMessage errorMessage)
    {
        ErrorMessages = [errorMessage];
    }
}
