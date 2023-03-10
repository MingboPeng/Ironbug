using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ironbug
{
    public static class IB_JsonSetting
	{
		private static JsonSerializerSettings _setting;
		public static JsonSerializerSettings ConvertSetting
		{
			get
			{

				if (_setting == null)
				{
                    
                    _setting = new JsonSerializerSettings
                    {
                        Converters = new List<JsonConverter>() {
                            IB_JsonConverter_FieldArgument.Instance,
                        },
                        TypeNameHandling = TypeNameHandling.Objects,
                        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                        ContractResolver = ContractResolver,
                        MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead
                    };
				}
                return _setting;
			}
		}

        private static Newtonsoft.Json.Serialization.IContractResolver _contractResolver;
        public static Newtonsoft.Json.Serialization.IContractResolver ContractResolver
        {
            get
            {
                if (_contractResolver == null)
                {
                    var res = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                    res.DefaultMembersSearchFlags = res.DefaultMembersSearchFlags | System.Reflection.BindingFlags.NonPublic;
                    _contractResolver = res;
                }
                return _contractResolver;
            }
        }
    }
}