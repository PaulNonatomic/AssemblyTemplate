using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AssemblyTemplate.Editor
{
	public class AssemblyTemplateWindow : EditorWindow
	{
		[SerializeField] 
		private VisualTreeAsset _tree;
		
		[SerializeField]
		private string _templatePath = "Packages/com.nonatomic.assembly-template/Editor/BasicAssemblyTemplate.json";

		private DirectoryStructureTemplateSO _template;
		private TextField _templatePathField;
		private TextField _assemblyNameField;
		private TextField _assemblyDomainField;
		private Button _generateButton;

		private static AssemblyTemplateWindow _window;

		private const string AssemblyName = "{ASSEMBLY_NAME}";
		private const string AssemblyDomain = "{ASSEMBLY_DOMAIN}";

		[MenuItem("Assets/Create/Assembly Structure", false, 100)]
		public static void ShowEditor()
		{
			_window = GetWindow<AssemblyTemplateWindow>();
			_window.titleContent = new GUIContent("Assembly Template");
		}

		private void CreateGUI()
		{
			_tree.CloneTree(rootVisualElement);

			InitFields();
			InitButtons();
		}

		private void InitFields()
		{
			_templatePathField = rootVisualElement.Q<TextField>("TemplatePath");
			_templatePathField.value = _templatePath;
				
			_assemblyNameField = rootVisualElement.Q<TextField>("AssemblyName");
			_assemblyDomainField = rootVisualElement.Q<TextField>("AssemblyDomain");
		}

		private void InitButtons()
		{
			_generateButton = rootVisualElement.Q<Button>("Generate");
			_generateButton.clicked += OnGenerate;
		}

		private void OnGenerate()
		{
			GenerateAssemblyStructure();
			
			_generateButton.clicked -= OnGenerate;
			_window.Close();
		}

		private void GenerateAssemblyStructure()
		{
			LoadTemplate();
			ProcessTemplate();
			
			AssetDatabase.Refresh();
		}

		private void ProcessTemplate()
		{
			var selectedDirectory = AssetDatabase.GetAssetPath(Selection.activeObject);
			if (!AssetDatabase.IsValidFolder(selectedDirectory)) return;
			
			CreateRecursiveSubDirectories(selectedDirectory, _template.Root);
		}
		
		private void CreateRecursiveSubDirectories(string path, DirectoryTemplate directory)
		{
			var directoryName = ProcessName(directory.Name);
			var childPath = CreateDirectory(path, directoryName);
			CreateAsmdefFile(childPath, directory);
			
			foreach(var file in directory.Files)
			{
				CreateFile(childPath, file);
			}

			foreach(var child in directory.Children)
			{
				CreateRecursiveSubDirectories(childPath, child);
			}
		}

		private string ProcessName(string value)
		{
			value = value.Replace(AssemblyName, _assemblyNameField.value);
			value = value.Replace(AssemblyDomain, _assemblyDomainField.value);

			return value;
		}

		private string ProcessContent(string value)
		{
			value = ProcessName(value);

			return value;
		}

		private void LoadTemplate()
		{
			var path = _templatePathField.value;
			var json = File.ReadAllText(path);
			_template = JsonConvert.DeserializeObject<DirectoryStructureTemplateSO>(json);
		}

		private string CreateDirectory(string path, string name)
		{
			var guid = AssetDatabase.CreateFolder(path, name);
			return AssetDatabase.GUIDToAssetPath(guid);
		}

		private void CreateAsmdefFile(string path, DirectoryTemplate directory)
		{
			if(directory.Asmdef == null) return;
			if (string.IsNullOrEmpty(directory.Asmdef.Name)) return;

			directory.Asmdef.Name = ProcessName(directory.Asmdef.Name);
			directory.Asmdef.RootNameSpace = ProcessName(directory.Asmdef.RootNameSpace);
			
			var jsonString = JsonConvert.SerializeObject(directory.Asmdef, Formatting.Indented);
			var pathToSave = Path.Combine(path, $"{directory.Asmdef.Name}.asmdef");
			
			File.WriteAllText(pathToSave, jsonString);
		}

		private void CreateFile(string path, FileTemplate file)
		{
			if(file == null) return;
			if (string.IsNullOrEmpty(file.Name)) return;
			
			file.Name = ProcessName(file.Name);
			file.Content = ProcessContent(file.Content);
			var pathToSave = Path.Combine(path, $"{file.Name}");
			File.WriteAllText(pathToSave,  file.Content);
		}
	}
}