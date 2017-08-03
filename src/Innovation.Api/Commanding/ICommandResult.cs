namespace Innovation.Api.Commanding
{
    /// <summary>
    /// This is the interface all command results must implement
    /// </summary>
    public interface ICommandResult
    {
        /// <summary>
        /// This should be set to true if the intended operation was succesful
        /// </summary>
        bool Success { get; }
    }
}
