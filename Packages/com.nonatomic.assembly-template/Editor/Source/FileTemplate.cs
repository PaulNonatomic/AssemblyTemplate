using Newtonsoft.Json;

namespace AssemblyTemplate.Editor
{
	public class FileTemplate
	{
		[JsonProperty("name")]
		public string Name;

		[JsonProperty("content")] 
		public string Content;
	}
}