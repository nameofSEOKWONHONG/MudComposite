﻿namespace MudMvvMKit;

public class Results
{
    /// <summary>
    /// 결과 메세지 목록
    /// </summary>
    public IEnumerable<string> Messages { get; set; }

    /// <summary>
    /// 정합성 체크 결과
    /// </summary>
    public Dictionary<string, string> ValidateResults { get; set; }
        = new Dictionary<string, string>();

    public Results()
    {
    }

    /// <summary>
    /// 요청 성공 여부, false라면 ValidateResults에 내역이 있음. empty는  true로 처리함.
    /// </summary>
    public bool Succeeded { get; set; }

    public static Results Fail()
    {
        return new Results { Succeeded = false };
    }

    public static Results Fail(string message)
    {
        return new Results { Succeeded = false, Messages = [message] };
    }

    public static Results Fail(IEnumerable<string> messages)
    {
        return new Results { Succeeded = false, Messages = messages };
    }

    public static Results Fail(Dictionary<string, string> validate)
    {
        return new Results() { Succeeded = false, ValidateResults = validate };
    }

    public static Task<Results> FailAsync()
    {
        return Task.FromResult(Fail());
    }

    public static Task<Results> FailAsync(string message)
    {
        return Task.FromResult(Fail(message));
    }

    public static Task<Results> FailAsync(IEnumerable<string> messages)
    {
        return Task.FromResult(Fail(messages));
    }

    public static Task<Results> FailAsync(Dictionary<string, string> errors)
    {
        return Task.FromResult(Fail(errors));
    }

    public static Results Success()
    {
        return new Results { Succeeded = true, Messages = ["Success."] };
    }

    public static Results Success(string message)
    {
        return new Results { Succeeded = true, Messages = ["Success.", message] };
    }

    public static Task<Results> SuccessAsync()
    {
        return Task.FromResult(Success());
    }

    public static Task<Results> SuccessAsync(string message)
    {
        return Task.FromResult(Success(message));
    }
}

public class Results<T> : Results
{
    public Results()
    {
    }

    /// <summary>
    /// 결과 데이터
    /// </summary>
    public T Data { get; set; }

    public new static Results<T> Fail()
    {
        return new Results<T> { Succeeded = false };
    }

    public new static Results<T> Fail(string message)
    {
        return new Results<T> { Succeeded = false, Messages = [message] };
    }

    public static Results<T> Fail(List<string> messages)
    {
        return new Results<T> { Succeeded = false, Messages = messages };
    }

    public new static Results<T> Fail(Dictionary<string, string> errors)
    {
        return new Results<T>() { Succeeded = false, ValidateResults = errors };
    }

    public static Results<T> Fail(T item)
    {
        return new Results<T>() { Succeeded = false, Data = item};
    }

    public new static Task<Results<T>> FailAsync()
    {
        return Task.FromResult(Fail());
    }

    public new static Task<Results<T>> FailAsync(string message)
    {
        return Task.FromResult(Fail(message));
    }

    public static Task<Results<T>> FailAsync(List<string> messages)
    {
        return Task.FromResult(Fail(messages));
    }

    public new static Task<Results<T>> FailAsync(Dictionary<string, string> errors)
    {
        return Task.FromResult(Fail(errors));
    }
    
    public static Task<Results<T>> FailAsync(T data)
    {
        return Task.FromResult(Fail(data));
    }

    public new static Results<T> Success()
    {
        return new Results<T> { Succeeded = true, Messages = ["Success."] };
    }

    public new static Results<T> Success(string message)
    {
        return new Results<T> { Succeeded = true, Messages = ["Success.", message] };
    }

    public static Results<T> Success(T data)
    {
        return new Results<T> { Succeeded = true, Data = data, Messages = ["Success."] };
    }

    public static Results<T> Success(T data, string message)
    {
        return new Results<T> { Succeeded = true, Data = data, Messages = ["Success.", message] };
    }

    public static Results<T> Success(T data, List<string> messages)
    {
        messages.Insert(0, "Search Success.");
        return new Results<T> { Succeeded = true, Data = data, Messages = messages };
    }

    public new static Task<Results<T>> SuccessAsync()
    {
        return Task.FromResult(Success());
    }

    public static Task<Results<T>> SuccessAsync(T data)
    {
        return Task.FromResult(Success(data));
    }

    public static Task<Results<T>> SuccessAsync(T data, string message)
    {
        return Task.FromResult(Success(data, message));
    }
}

public class PaginatedResult<T> : Results<T>
{
    public PaginatedResult(List<T> datum)
    {
        Datum = datum;
    }

    public IEnumerable<T> Datum { get; set; }

    internal PaginatedResult(bool succeeded, IEnumerable<T> datum = default, List<string> messages = null, int count = 0, int page = 1, int pageSize = 10)
    {
        Datum = datum;
        PageNo = page;
        Succeeded = succeeded;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
    }

    public static PaginatedResult<T> Fail()
    {
        return Fail(new List<string>() { });
    }
    
    public static PaginatedResult<T> Fail(string message)
    {
        return Fail(new List<string>() { message });
    }

    public static PaginatedResult<T> Fail(List<string> messages)
    {
        return new PaginatedResult<T>(false, default, messages);
    }

    public static Task<PaginatedResult<T>> FailAsync()
    {
        return FailAsync(new List<string>() { });
    }

    public static Task<PaginatedResult<T>> FailAsync(string message)
    {
        return FailAsync(new List<string>() { message });
    }

    public static Task<PaginatedResult<T>> FailAsync(List<string> messages)
    {
        return Task.FromResult(new PaginatedResult<T>(false, default, messages));
    }

    public static PaginatedResult<T> Success(IEnumerable<T> data, int totalCount, int currentPage, int pageSize)
    {
        var result = new PaginatedResult<T>(true, data, null, totalCount, currentPage, pageSize);
        result.Messages = new List<string>() { "Success." };
        return result;
    }

    public static Task<PaginatedResult<T>> SuccessAsync(IEnumerable<T> data, int totalCount, int currentPage,
        int pageSize)
    {
        return Task.FromResult(Success(data, totalCount, currentPage, pageSize));
    }

    public int PageNo { get; set; }

    public int TotalPages { get; set; }

    public int TotalCount { get; set; }
    public int PageSize { get; set; }

    public bool HasPreviousPage => PageNo > 1;

    public bool HasNextPage => PageNo < TotalPages;
}