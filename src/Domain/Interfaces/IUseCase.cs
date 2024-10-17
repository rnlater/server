using Shared.Types;

namespace Domain.Interfaces;

public interface IUseCase<SuccessType, Params>
{
    Task<Result<SuccessType>> Execute(Params parameters);
}

public class NoParam
{
    public static NoParam Value { get; } = new NoParam();
}