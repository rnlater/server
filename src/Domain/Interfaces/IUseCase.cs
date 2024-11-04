using Shared.Types;

namespace Domain.Interfaces;

public interface IUseCase<SuccessType, Params>
{
    /// <summary>
    /// Execute the use case
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task<Result<SuccessType>> Execute(Params parameters);
}

public class NoParam
{
    /// <summary>
    /// Singleton instance of NoParam
    /// </summary>
    public static NoParam Value { get; } = new NoParam();
}

public enum PivotSuccessModificationType
{
    Created,
    Deleted
}