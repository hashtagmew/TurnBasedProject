//using UnityEngine;
//using UnityEditor;
//
//using System; 
//
//[InitializeOnLoad]
//public class ProjectUpdateChecker : EditorWindow {
//
//	public static string s_sCurrentVersion = "";
//
//	public static int s_iMajorVersion;
//	public static int s_iMinorVersion;
//
//	public static WWW s_wwwVersion;
//	public static bool s_bUpdateCheckComplete = false;
//	public static TextAsset verfile;
//
//	static ProjectUpdateChecker() {
//		s_bUpdateCheckComplete = false;
//
//		verfile = (TextAsset)Resources.Load("project-version");
//		s_sCurrentVersion = verfile.text;
//
//		//s_sCurrentVersion = EditorPrefs.GetString("TBS: ProjVersion", "0");
//		
//		s_wwwVersion = new WWW("https://raw.githubusercontent.com/hashtagmew/TurnBasedProject/master/TBS_Test/Assets/Resources/project-version.txt");
//
//		EditorApplication.update += Update;
//	}
//
//	static void Update() {
//		if (!String.IsNullOrEmpty(s_wwwVersion.error) && !s_bUpdateCheckComplete) {
//			s_bUpdateCheckComplete = true;
//
//			Debug.Log ("Error checking version status: " + s_wwwVersion.error);
//		}
//		else if (s_wwwVersion.isDone && !s_bUpdateCheckComplete) {
//			s_bUpdateCheckComplete = true;
//			
//			Debug.Log("Your project version is v" + s_sCurrentVersion + 
//			          "\nThe GitHub version is   v" + s_wwwVersion.text);
//		}
//	}
//
//	void OnHierarchyChange() {
//		EditorApplication.ExecuteMenuItem("Assets/Sync MonoDevelop Project");
//		Debug.Log("Sync");
//	}
//
//	void OnProjectChange() {
//		EditorApplication.ExecuteMenuItem("Assets/Sync MonoDevelop Project");
//		Debug.Log("Sync");
//	}
//}
