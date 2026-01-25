namespace wf_cloudzone_gen_api.helper
{
    using Google.Protobuf.WellKnownTypes;
    using System.Collections.Generic;
    using System.Linq;

    public static class ProtobufExtensions
    {
        public static Value ToProtoValue(this string str)
            => new Value { StringValue = str };

        public static Value ToProtoList(this IEnumerable<string> list)
            => new Value
            {
                ListValue = new ListValue
                {
                    Values = { list.Select(l => new Value { StringValue = l }) }
                }
            };
    }

}
