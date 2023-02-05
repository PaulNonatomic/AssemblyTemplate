using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace AssemblyTemplate.Editor
{
	[CreateAssetMenu(fileName = "DirectoryStructureTemplateSO", menuName = "Assembly Template/Directory Structure Template", order = 0)]
	public class DirectoryStructureTemplateSO : ScriptableObject
	{
		[JsonProperty("root")]
		public DirectoryTemplate Root = new ();
	}
}