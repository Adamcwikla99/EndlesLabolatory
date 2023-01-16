using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(Readme)), InitializeOnLoad]
public class ReadmeEditor : Editor
{
	#region Serialized Fields

	[SerializeField]
	private GUIStyle m_LinkStyle;

	[SerializeField]
	private GUIStyle m_TitleStyle;

	[SerializeField]
	private GUIStyle m_HeadingStyle;

	[SerializeField]
	private GUIStyle m_BodyStyle;

	[SerializeField]
	private GUIStyle m_ButtonStyle;

	#endregion
	#region Private Fields

	private static readonly string s_ShowedReadmeSessionStateName = "ReadmeEditor.showedReadme";
	private static readonly string s_ReadmeSourceDirectory = "Assets/TutorialInfo";

	private bool m_Initialized;

	#endregion
	#region Constants

	private const float k_Space = 16f;

	#endregion

	private GUIStyle LinkStyle {
		get { return this.m_LinkStyle; }
	}

	private GUIStyle TitleStyle {
		get { return this.m_TitleStyle; }
	}

	private GUIStyle HeadingStyle {
		get { return this.m_HeadingStyle; }
	}

	private GUIStyle BodyStyle {
		get { return this.m_BodyStyle; }
	}

	private GUIStyle ButtonStyle {
		get { return this.m_ButtonStyle; }
	}
	#region Constructors

	static ReadmeEditor()
	{
		EditorApplication.delayCall += SelectReadmeAutomatically;
	}

	#endregion
	#region Public Methods

	public override void OnInspectorGUI()
	{
		Readme readme = (Readme)this.target;
		this.Init();

		foreach (Readme.Section section in readme.sections)
		{
			if (!string.IsNullOrEmpty(section.heading))
			{
				GUILayout.Label(section.heading, this.HeadingStyle);
			}

			if (!string.IsNullOrEmpty(section.text))
			{
				GUILayout.Label(section.text, this.BodyStyle);
			}

			if (!string.IsNullOrEmpty(section.linkText))
			{
				if (this.LinkLabel(new GUIContent(section.linkText)))
				{
					Application.OpenURL(section.url);
				}
			}

			GUILayout.Space(k_Space);
		}

		if (GUILayout.Button("Remove Readme Assets", this.ButtonStyle))
		{
			RemoveTutorial();
		}
	}

	#endregion
	#region Protected Methods

	protected override void OnHeaderGUI()
	{
		Readme readme = (Readme)this.target;
		this.Init();

		float iconWidth = Mathf.Min(EditorGUIUtility.currentViewWidth / 3f - 20f, 128f);

		GUILayout.BeginHorizontal("In BigTitle");
		{
			if (readme.icon != null)
			{
				GUILayout.Space(k_Space);
				GUILayout.Label(readme.icon, GUILayout.Width(iconWidth), GUILayout.Height(iconWidth));
			}

			GUILayout.Space(k_Space);
			GUILayout.BeginVertical();
			{

				GUILayout.FlexibleSpace();
				GUILayout.Label(readme.title, this.TitleStyle);
				GUILayout.FlexibleSpace();
			}

			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
		}

		GUILayout.EndHorizontal();
	}

	#endregion
	#region Private Methods

	private static void RemoveTutorial()
	{
		if (EditorUtility.DisplayDialog("Remove Readme Assets", $"All contents under {s_ReadmeSourceDirectory} will be removed, are you sure you want to proceed?", "Proceed", "Cancel"))
		{
			if (Directory.Exists(s_ReadmeSourceDirectory))
			{
				_ = FileUtil.DeleteFileOrDirectory(s_ReadmeSourceDirectory);
				_ = FileUtil.DeleteFileOrDirectory(s_ReadmeSourceDirectory + ".meta");
			}
			else
			{
				Debug.Log($"Could not find the Readme folder at {s_ReadmeSourceDirectory}");
			}

			Readme readmeAsset = SelectReadme();

			if (readmeAsset != null)
			{
				string path = AssetDatabase.GetAssetPath(readmeAsset);
				_ = FileUtil.DeleteFileOrDirectory(path + ".meta");
				_ = FileUtil.DeleteFileOrDirectory(path);
			}

			AssetDatabase.Refresh();
		}
	}

	private static void SelectReadmeAutomatically()
	{
		if (!SessionState.GetBool(s_ShowedReadmeSessionStateName, false))
		{
			Readme readme = SelectReadme();
			SessionState.SetBool(s_ShowedReadmeSessionStateName, true);

			if (readme && !readme.loadedLayout)
			{
				LoadLayout();
				readme.loadedLayout = true;
			}
		}
	}

	private static void LoadLayout()
	{
		Assembly assembly = typeof(EditorApplication).Assembly;
		Type windowLayoutType = assembly.GetType("UnityEditor.WindowLayout", true);
		MethodInfo method = windowLayoutType.GetMethod("LoadWindowLayout", BindingFlags.Public | BindingFlags.Static);
		_ = method.Invoke(null, new object[]
		{
			Path.Combine(Application.dataPath, "TutorialInfo/Layout.wlt"), false
		});
	}

	private static Readme SelectReadme()
	{
		string[] ids = AssetDatabase.FindAssets("Readme t:Readme");

		if (ids.Length == 1)
		{
			Object readmeObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(ids[0]));

			Selection.objects = new[]
			{
				readmeObject
			};

			return (Readme)readmeObject;
		}

		Debug.Log("Couldn't find a readme");

		return null;
	}

	private void Init()
	{
		if (this.m_Initialized)
		{
			return;
		}

		this.m_BodyStyle = new GUIStyle(EditorStyles.label)
		{
			wordWrap = true,
			fontSize = 14,
			richText = true
		};

		this.m_TitleStyle = new GUIStyle(this.m_BodyStyle)
		{
			fontSize = 26
		};

		this.m_HeadingStyle = new GUIStyle(this.m_BodyStyle)
		{
			fontStyle = FontStyle.Bold,
			fontSize = 18
		};

		this.m_LinkStyle = new GUIStyle(this.m_BodyStyle)
		{
			wordWrap = false
		};

		// Match selection color which works nicely for both light and dark skins
		this.m_LinkStyle.normal.textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f);
		this.m_LinkStyle.stretchWidth = false;

		this.m_ButtonStyle = new GUIStyle(EditorStyles.miniButton)
		{
			fontStyle = FontStyle.Bold
		};

		this.m_Initialized = true;
	}

	private bool LinkLabel(GUIContent label, params GUILayoutOption[] options)
	{
		Rect position = GUILayoutUtility.GetRect(label, this.LinkStyle, options);

		Handles.BeginGUI();
		Handles.color = this.LinkStyle.normal.textColor;
		Handles.DrawLine(new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
		Handles.color = Color.white;
		Handles.EndGUI();

		EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);

		return GUI.Button(position, label, this.LinkStyle);
	}

	#endregion
}
