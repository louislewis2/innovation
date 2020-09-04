namespace Innovation.Api.Core
{
    public interface ICorrelationAware
    {
        string CorrelationId { set; }
    }
}
