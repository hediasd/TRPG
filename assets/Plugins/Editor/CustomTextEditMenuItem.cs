    using UnityEngine;
    using UnityEditor;
    using System.IO;
     
    public static class CustomTextEditMenuItem {
     
        const string TextEditor = "C:\\Program Files (x86)\\Notepad++\\notepad++.exe"; // (Example)
     
        [MenuItem("Assets/Open in Text Editor...")]
        static void OpenInTextEditor() {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), AssetDatabase.GetAssetPath(Selection.activeObject));
            System.Diagnostics.Process.Start(TextEditor, filePath);
        }
     
      [MenuItem("Assets/Open in Text Editor...", true)]
      static bool ValidateOpenInTextEditor() {
            return AssetDatabase.GetAssetPath(Selection.activeObject).EndsWith(".txt", System.StringComparison.OrdinalIgnoreCase);
        }
    }
