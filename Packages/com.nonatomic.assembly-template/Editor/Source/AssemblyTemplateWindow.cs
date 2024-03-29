using System.IO;
using AssemblyTemplate.Editor;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AssemblyTemplate.Tests.Editor.Editor.Source
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

		private const string AssemblyNameKey = "{{ASSEMBLY_NAME}}";
		private const string AssemblyDomainKey = "{{ASSEMBLY_DOMAIN}}";

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
			
			TemplateGenerator.Generate(selectedDirectory, _assemblyNameField.value, string.Empty, _assemblyDomainField.value, _template);
		}
		
		private void LoadTemplate()
		{
			var path = _templatePathField.value;
			var json = File.ReadAllText(path);
			_template = JsonConvert.DeserializeObject<DirectoryStructureTemplateSO>(json);
		}
	}
}