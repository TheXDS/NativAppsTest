namespace NativApps.Core.Services;

/// <summary>
/// Describes the result of an operation.
/// </summary>
public class ServiceResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceResult"/> class.
    /// </summary>
    /// <param name="success">Value that indicates either success or failure to perform the operation.</param>
    /// <param name="message">
    /// Additional message that describes the result of the operation.
    /// </param>
    public ServiceResult(bool success, string? message = null)
    {
        Success = success;
        Message = message;
    }

    /// <summary>
    /// Indicates either success or failure to perform the operation.
    /// </summary>
    public bool Success { get; }

    /// <summary>
    /// Additional message that describes the result of the operation.
    /// </summary>
    public string? Message { get; }

    /// <summary>
    /// Implicitly converts a <see cref="bool"/> value to a
    /// <see cref="ServiceResult{T}"/>.
    /// </summary>
    /// <param name="result">Operation result.</param>
    public static implicit operator ServiceResult(bool success) => new(success);
}
