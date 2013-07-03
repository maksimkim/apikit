namespace ApiKit.Contracts
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public interface IKitSettings
    {
        [DefaultValue(false)]
        bool SslOffload { get; }

        [DefaultValue(false)]
        bool DetailedErrors { get; }

        [DefaultValue(new string[]{})]
        IEnumerable<string> ValidationExcludes { get; set; }
    }
}