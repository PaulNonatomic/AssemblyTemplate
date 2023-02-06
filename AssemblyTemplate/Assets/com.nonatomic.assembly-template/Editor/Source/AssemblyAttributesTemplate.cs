using Unity.Plastic.Newtonsoft.Json;

namespace AssemblyTemplate.Editor
{
	public class AssemblyAttributesTemplate
	{
		[JsonProperty("name")]
		public string Name;

		[JsonProperty("content")]
		public string Content;
	}
}