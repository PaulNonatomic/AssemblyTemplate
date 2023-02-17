using System.IO;
using AssemblyTemplate.Editor;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AssemblyTemplate.Tests.EditMode
{
	public class TemplateDeserializationTests
	{
		private string _basicAssemblyTemplatePath = "Packages/com.nonatomic.assembly-template/Editor/BasicAssemblyTemplate.json";
		private string _packageTemplatePath = "Packages/com.nonatomic.assembly-template/Editor/NewPackageTemplate.json";
		private string _gameAssemblyTemplatePath = "Packages/com.nonatomic.assembly-template/Editor/GameAssemblyTemplate.json";
		
		[Test]
		public void Deserialize_The_Assembly_Template()
		{
			var json = File.ReadAllText(_basicAssemblyTemplatePath);
			var template = JsonConvert.DeserializeObject<DirectoryStructureTemplateSO>(json);
		}
		
		[Test]
		public void Deserialize_The_Game_Template()
		{
			var json = File.ReadAllText(_gameAssemblyTemplatePath);
			var template = JsonConvert.DeserializeObject<DirectoryStructureTemplateSO>(json);
		}
		
		[Test]
		public void Deserialize_The_Package_Template()
		{
			var json = File.ReadAllText(_packageTemplatePath);
			var template = JsonConvert.DeserializeObject<DirectoryStructureTemplateSO>(json);
		}
	}
}