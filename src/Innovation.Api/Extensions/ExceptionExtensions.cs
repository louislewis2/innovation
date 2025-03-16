namespace System
{
    using System.Diagnostics.CodeAnalysis;

    public static class ExceptionExtensions
    {
        public static string GetInnerMostMessage([DisallowNull] this Exception ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            return ex.Message;
        }
    }
}