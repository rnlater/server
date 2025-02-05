using Shared.Constants;

namespace Shared.Types;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public Paging? Paging { get; }
    private Failure? Failure { get; }
    public List<string> Errors => Failure?.ErrorMessages.Select(e => e.ToString()).ToList() ?? [];
    public ErrorMessage Error => Failure?.ErrorMessages.First() ?? default!;

    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
        Failure = null;
    }

    private Result(T value, Paging paging)
    {
        IsSuccess = true;
        Value = value;
        Paging = paging;
        Failure = null;
    }

    private Result(Failure failure)
    {
        IsSuccess = false;
        Value = default!;
        Failure = failure;
    }

    public static Result<T> Done(T value) => new(value);
    public static Result<T> Done(T value, Paging paging) => new(value, paging);

    public static Result<T> Fail(Failure failure) => new(failure);
    public static Result<T> Fail(ErrorMessage error) => new(new Failure(error));
    public static Result<T> Fail(List<ErrorMessage> error) => new(new Failure(error));
}

public class Paging
{
    public int Page { get; }
    public int PageSize { get; }
    public int TotalElements { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalElements / (double)PageSize);

    public Paging(int page, int pageSize, int totalElements)
    {
        Page = page;
        PageSize = pageSize;
        TotalElements = totalElements;
    }
}

public enum PivotSuccessModificationType
{
    Created,
    Deleted
}