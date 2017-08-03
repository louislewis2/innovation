namespace System
{
    public static class ExceptionExtensions
    {
        public static string GetInnerMostMessage(this Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            return ex.Message;
        }
    }
}