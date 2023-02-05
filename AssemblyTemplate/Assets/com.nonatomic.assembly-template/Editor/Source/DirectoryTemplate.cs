using System;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine.Serialization;

namespace AssemblyTemplate.Editor
{
	[Serializable]
	public class DirectoryTemplate
	{
		[JsonProperty("name")]
		public string Name;

		[JsonProperty("children")]
		public List<DirectoryTemplate> Children = new ();
		
		[JsonProperty("asmdef")]
		public AssemblyDefinitionTemplate Asmdef = new ();
		
		[JsonProperty("files")]
		public List<FileTemplate> Files = new ();
	}
}