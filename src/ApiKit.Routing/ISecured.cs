namespace ApiKit.Routing
{
    public interface ISecured
    {
        bool? Private { get; }

        bool? Secure { get; }
    }
}