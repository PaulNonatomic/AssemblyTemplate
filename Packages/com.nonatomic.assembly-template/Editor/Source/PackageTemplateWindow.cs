using System;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AssemblyTemplate.Editor
{
	public class PackageTemplateWindow : EditorWindow
	{
		[SerializeField] 
		private VisualTreeAsset _tree;
		
		[SerializeField]
		private string _templatePath = "Packages/com.nonatomic.assembly-template/Editor/NewPackageTemplate.json";

		private DirectoryStructureTemplateSO _template;
		private TextField _templatePathField;
		private TextField _packageNameField;
		private TextField _packageDomainField;
		private Button _generateButton;

		private static PackageTemplateWindow _window;
		private TextField _packageVersionField;
		private TextField _authorNameField;
		private TextField _authorEmail;
		private TextField _authorUrl;
		private TextField _packageType;
		private TextField _packageDescription;
		private TextField _packageRepo;

		private const string PackageNameKey = "{{PACKAGE_NAME}}";
		private const string PackageDomainKey = "{{PACKAGE_DOMAIN}}";
		private const string PackageVersionKey = "{{PACKAGE_VERSION}}";
		private const string PackageDescriptionKey = "{{PACKAGE_DESCRIPTION}}";
		private const string PackageRepoKey = "{{PACKAGE_REPO}}";
		private const string AssemblyNameKey = "{{ASSEMBLY_NAME}}";
		private const string AssemblyDomainKey = "{{ASSEMBLY_DOMAIN}}";
		private const string DateKey = "{{DATE}}";
		private const string AuthorNameKey = "{{AUTHOR_NAME}}";
		private const string AuthorEmailKey = "{{AUTHOR_EMAIL}}";
		private const string AuthorUrlKey = "{{AUTHOR_URL}}";

		[MenuItem("Assets/Create/New Package", false, 100)]
		public static void ShowEditor()
		{
			_window = GetWindow<PackageTemplateWindow>();
			_window.titleContent = new GUIContent("Package Template");
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
				
			_packageNameField = rootVisualElement.Q<TextField>("PackageName");
			_packageDomainField = rootVisualElement.Q<TextField>("PackageDomain");
			_packageVersionField = rootVisualElement.Q<TextField>("PackageVersion");
			_packageDescription = rootVisualElement.Q<TextField>("PackageDescription");
			_packageRepo = rootVisualElement.Q<TextField>("PackageRepo");
			
			_authorNameField = rootVisualElement.Q<TextField>("AuthorName");
			_authorEmail = rootVisualElement.Q<TextField>("AuthorEmail");
			_authorUrl = rootVisualElement.Q<TextField>("AuthorUrl");
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
			var selectedDirectory = Path.GetFullPath("Packages/");
			
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
			value = value.Replace(PackageNameKey, _packageNameField.value);
			value = value.Replace(PackageDomainKey, _packageDomainField.value);
			value = value.Replace(PackageVersionKey, _packageVersionField.value);
			value = value.Replace(PackageDescriptionKey, _packageDescription.value);
			value = value.Replace(PackageRepoKey, _packageRepo.value);
			
			value = value.Replace(AssemblyNameKey, _packageNameField.value);
			value = value.Replace(AssemblyDomainKey, _packageDomainField.value);
			
			value = value.Replace(AuthorEmailKey, _authorEmail.value);
			value = value.Replace(AuthorNameKey, _authorNameField.value);
			value = value.Replace(AuthorUrlKey, _authorUrl.value);

			var dateString = DateTime.Now.ToString("yyyy-MM-dd");
			value = value.Replace(DateKey, dateString);
			

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
			var fullPath = Path.Combine(path, name);
			var guid = Directory.CreateDirectory(fullPath);
			return fullPath;
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