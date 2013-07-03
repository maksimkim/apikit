namespace ApiKit.Contracts.Validation
{
    using System.Runtime.Serialization;

    [DataContract(Name = "error")]
    public class ValidationErrorResponse
    {
        [DataMember(Name = "code")]
        public int Code { get; set; }

        [DataMember(Name = "src")]
        public string Source { get; set; }

        [DataMember(Name = "desc")]
        public string Description { get; set; }

        [DataMember(Name = "details")]
        public string Details { get; set; }
    }
}