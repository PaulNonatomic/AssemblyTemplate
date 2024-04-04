using System.IO;
using AssemblyTemplate.Tests.Editor.Editor.Source;
using Newtonsoft.Json;
using UnityEditor;

namespace AssemblyTemplate.Editor
{
	public class TemplateGenerator
	{
		private const string AssemblyNameKey = "{{ASSEMBLY_NAME}}";
		private const string AssemblyDomainKey = "{{ASSEMBLY_DOMAIN}}";
		
		private static string _assemblyNameField = string.Empty;
		private static string _assemblyDomainField = string.Empty;
		private static string _assetBasePath;

		public static void Generate(string path, string assemblyName, string assemblyDomain, string assetBasePath, DirectoryStructureTemplateSO template)
		{
			_assemblyNameField = assemblyName.Replace(" ", "");
			_assemblyDomainField = assemblyDomain.Replace(" ", "");
			_assetBasePath = assetBasePath;

			CreateRecursiveSubDirectories(path, template.Root);
		}
		
		private static void CreateRecursiveSubDirectories(string path, DirectoryTemplate directory)
		{
			var directoryName = ProcessName(directory.Name);
			var childPath = CreateDirectory(path, directoryName);
			CreateAsmdefFile(childPath, directory);
			
			foreach(var file in directory.Files)
			{
				CreateFile(childPath, file);
			}

			foreach (var asset in directory.Assets)
			{
				CreateAsset(childPath, asset);
			}

			foreach(var child in directory.Children)
			{
				CreateRecursiveSubDirectories(childPath, child);
			}
		}
		
		private static string ProcessName(string value)
		{
			value = value.Replace(AssemblyNameKey, _assemblyNameField);
			value = value.Replace(AssemblyDomainKey, _assemblyDomainField);

			return value;
		}

		private static string ProcessContent(string value)
		{
			value = ProcessName(value);

			return value;
		}
		
		private static string CreateDirectory(string path, string name)
		{
			var guid = AssetDatabase.CreateFolder(path, name);
			return AssetDatabase.GUIDToAssetPath(guid);
		}

		private static void CreateAsmdefFile(string path, DirectoryTemplate directory)
		{
			if (directory.Asmdef == null) return;
			if (string.IsNullOrEmpty(directory.Asmdef.Name)) return;

			directory.Asmdef.Name = ProcessName(directory.Asmdef.Name);
			directory.Asmdef.RootNameSpace = ProcessName(directory.Asmdef.RootNameSpace);
			
			var jsonString = JsonConvert.SerializeObject(directory.Asmdef, Formatting.Indented);
			var pathToSave = Path.Combine(path, $"{directory.Asmdef.Name}.asmdef");
			File.WriteAllText(pathToSave, jsonString);
		}

		private static void CreateFile(string path, FileTemplate file)
		{
			if (file == null) return;
			if (string.IsNullOrEmpty(file.Name)) return;
			
			file.Name = ProcessName(file.Name);
			file.Content = ProcessContent(file.Content);
			var pathToSave = Path.Combine(path, $"{file.Name}");
			File.WriteAllText(pathToSave,  file.Content);
		}

		private static void CreateAsset(string path, AssetTemplate assetTemplate)
		{
			if (assetTemplate == null) return;
			if (string.IsNullOrEmpty(assetTemplate.AssetPath)) return;
			
			assetTemplate.AssetPath = ProcessName(assetTemplate.AssetPath);
			assetTemplate.NewName = ProcessName(assetTemplate.NewName);
			
			var pathToSave = Path.Combine(path, $"{assetTemplate.NewName}");
			var assetPath = Path.Combine(_assetBasePath, assetTemplate.AssetPath);
			
			AssetDatabase.CopyAsset(assetPath, pathToSave);
			
			if (assetTemplate.Open)
			{
				UnityEditor.EditorApplication.delayCall += () =>
				{
					var asset = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);
					AssetDatabase.OpenAsset(asset);
				};
			}
		}
	}
}