using Newtonsoft.Json;

namespace AssemblyTemplate.Tests.Editor.Editor.Source
{
	public class AssetTemplate
	{
		[JsonProperty("assetPath")]
		public string AssetPath;
		
		[JsonProperty("newName")]
		public string NewName;

		[JsonProperty("open")] 
		public bool Open;
	}
}