using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tutorial_Info.Scripts.Editor
{
	[CustomEditor(typeof(Readme))]
	[InitializeOnLoad]
	public class ReadmeEditor : UnityEditor.Editor {
	
		static string kShowedReadmeSessionStateName = "ReadmeEditor.showedReadme";
	
		static float kSpace = 16f;
	
		static ReadmeEditor()
		{
			EditorApplication.delayCall += SelectReadmeAutomatically;
		}
	
		static void SelectReadmeAutomatically()
		{
			if (!SessionState.GetBool(kShowedReadmeSessionStateName, false ))
			{
				var readme = SelectReadme();
				SessionState.SetBool(kShowedReadmeSessionStateName, true);
			
				if (readme && !readme.loadedLayout)
				{
					LoadLayout();
					readme.loadedLayout = true;
				}
			} 
		}
	
		static void LoadLayout()
		{
			var assembly = typeof(EditorApplication).Assembly; 
			var windowLayoutType = assembly.GetType("UnityEditor.WindowLayout", true);
			var method = windowLayoutType.GetMethod("LoadWindowLayout", BindingFlags.Public | BindingFlags.Static);
			method.Invoke(null, new object[]{Path.Combine(Application.dataPath, "About/Layout.wlt"), false});
		}
	
		[MenuItem("Help/Asset Store Originals/SNAPS/About")]
		static Readme SelectReadme() 
		{
			var ids = AssetDatabase.FindAssets("Readme t:Readme");
			if (ids.Length == 1)
			{
				var readmeObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(ids[0]));
			
				Selection.objects = new UnityEngine.Object[]{readmeObject};
			
				return (Readme)readmeObject;
			}
			else
			{
				Debug.Log("Couldn't find a readme");
				return null;
			}
		}
	
		protected override void OnHeaderGUI()
		{
			var readme = (Readme)target;
			Init();
		
			var iconWidth = Mathf.Min(EditorGUIUtility.currentViewWidth/3f - 20f, readme.iconMaxWidth);
		
			GUILayout.BeginHorizontal("In BigTitle");
			{
				GUILayout.Label(readme.icon, GUILayout.Width(iconWidth), GUILayout.Height(iconWidth));
				GUILayout.Label(readme.title, TitleStyle);
				GUILayout.Label(readme.titlesub, TitleStyle);
			}
			GUILayout.EndHorizontal();
		}
	
		public override void OnInspectorGUI()
		{
			var readme = (Readme)target;
			Init();
		
			foreach (var section in readme.sections)
			{
				if (!string.IsNullOrEmpty(section.heading))
				{
					GUILayout.Label(section.heading, HeadingStyle);
				}
				if (!string.IsNullOrEmpty(section.text))
				{
					GUILayout.Label(section.text, BodyStyle);
				}
				if (!string.IsNullOrEmpty(section.linkText))
				{
					GUILayout.Space(kSpace / 2);
					if (LinkLabel(new GUIContent(section.linkText)))
					{
						Application.OpenURL(section.url);
					}
				}
				GUILayout.Space(kSpace);
			}
		}
	
	
		bool mInitialized;
	
		GUIStyle LinkStyle { get { return mLinkStyle; } }
		[FormerlySerializedAs("m_LinkStyle")] [SerializeField] GUIStyle mLinkStyle;
	
		GUIStyle TitleStyle { get { return mTitleStyle; } }
		[FormerlySerializedAs("m_TitleStyle")] [SerializeField] GUIStyle mTitleStyle;
	
		GUIStyle HeadingStyle { get { return mHeadingStyle; } }
		[FormerlySerializedAs("m_HeadingStyle")] [SerializeField] GUIStyle mHeadingStyle;
	
		GUIStyle BodyStyle { get { return mBodyStyle; } }
		[FormerlySerializedAs("m_BodyStyle")] [SerializeField] GUIStyle mBodyStyle;
	
		void Init()
		{
			if (mInitialized)
				return;
			mBodyStyle = new GUIStyle(EditorStyles.label);
			mBodyStyle.wordWrap = true;
			mBodyStyle.fontSize = 14;
		
			mTitleStyle = new GUIStyle(mBodyStyle);
			mTitleStyle.fontSize = 24;

			mHeadingStyle = new GUIStyle(mBodyStyle);
			mHeadingStyle.fontSize = 18;
			mHeadingStyle.fontStyle = FontStyle.Bold; 
		
			mLinkStyle = new GUIStyle(mBodyStyle);
			// Match selection color which works nicely for both light and dark skins
			mLinkStyle.normal.textColor = new Color (0x00/255f, 0x78/255f, 0xDA/255f, 1f);
			mLinkStyle.stretchWidth = false;
		
			mInitialized = true;
		}
	
		bool LinkLabel (GUIContent label, params GUILayoutOption[] options)
		{
			var position = GUILayoutUtility.GetRect(label, LinkStyle, options);

			Handles.BeginGUI ();
			Handles.color = LinkStyle.normal.textColor;
			Handles.DrawLine (new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
			Handles.color = Color.white;
			Handles.EndGUI ();

			EditorGUIUtility.AddCursorRect (position, MouseCursor.Link);

			return GUI.Button (position, label, LinkStyle);
		}
	}
}

