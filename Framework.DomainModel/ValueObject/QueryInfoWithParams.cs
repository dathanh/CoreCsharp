namespace Framework.DomainModel.ValueObject
{
    public class QueryInfoWithParams : QueryInfo
    {
        public string ParameterDependencies { get; set; }
        public string Query { get; set; }
        public string IncludeIds { get; set; }
    }
}