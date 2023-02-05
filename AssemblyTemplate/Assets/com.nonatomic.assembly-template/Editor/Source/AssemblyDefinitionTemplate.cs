using System;
using Unity.Plastic.Newtonsoft.Json;

namespace AssemblyTemplate.Editor
{
	[Serializable]
	public class AssemblyDefinitionTemplate
	{
		[JsonProperty("name")]
		public string Name;
			
		[JsonProperty("rootNamespace")]
		public string RootNameSpace;
			
		[JsonProperty("references")]
		public string[] References;
			
		[JsonProperty("includePlatforms")]
		public string[] IncludePlatforms;
			
		[JsonProperty("excludePlatforms")]
		public string[] ExcludePlatforms;
			
		[JsonProperty("allowUnsafeCode")]
		public bool AllowUnsafeCode;
			
		[JsonProperty("overrideReferences")]
		public bool OverrideReferences;
			
		[JsonProperty("precompiledReferences")]
		public string[] PrecompiledReferences;
			
		[JsonProperty("autoReferenced")]
		public bool AutoReferenced = true;
			
		[JsonProperty("defineConstraints")]
		public string[] DefineConstraints;
			
		[JsonProperty("versionDefines")]
		public string[] VersionDefines;
			
		[JsonProperty("noEngineReferences")]
		public bool NoEngineReferences;
	}
}