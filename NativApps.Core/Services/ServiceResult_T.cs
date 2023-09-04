namespace NativApps.Core.Services;

/// <summary>
/// Represents the result of a service operation that returns data.
/// </summary>
/// <typeparam name="T">Type of data to be returned.</typeparam>
public class ServiceResult<T> : ServiceResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceResult{T}"/> class.
    /// </summary>
    /// <param name="result">Result value.</param>
    /// <param name="message">
    /// Additional message that describes the result of the operation.
    /// </param>
    public ServiceResult(T result, string? message = null) : base(true, message)
    {
        Result = result;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceResult{T}"/> class.
    /// </summary>
    /// <param name="success">
    /// Value that indicates either success or failure to perform the requested
    /// operation.
    /// </param>
    /// <param name="message">
    /// Additional message that describes the result of the operation.
    /// </param>
    public ServiceResult(bool success, string? message = null) : base(success, message)
    {
        Result = default!;
    }

    /// <summary>
    /// Gets the operation result.
    /// </summary>
    public T Result { get; }

    /// <summary>
    /// Implicitly converts a <typeparamref name="T"/> value to a
    /// <see cref="ServiceResult{T}"/>.
    /// </summary>
    /// <param name="result">Operation result.</param>
    public static implicit operator ServiceResult<T>(T result) => new(result);
}