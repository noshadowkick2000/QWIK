using System;
using UnityEngine;

namespace Tutorial_Info.Scripts
{
	public class Readme : ScriptableObject {
		public Texture2D icon;
		public float iconMaxWidth = 128f;
		public string title;
		public string titlesub;
		public Section[] sections;
		public bool loadedLayout;
	
		[Serializable]
		public class Section {
			public string heading, text, linkText, url;
		}
	}
}
