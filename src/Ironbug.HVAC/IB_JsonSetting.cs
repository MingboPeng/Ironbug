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
							new IB_JsonConverter_FieldArgSet(),
							new IB_JsonConverter_Children(),
							new IB_JsonConverter_HVACObjects()
						}
					};
				}
				return _setting;
			}
		}

	}
}