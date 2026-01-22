using UnityEngine;
using UnityEditor;
using TMPro;

/// <summary>
/// シーン内のすべてのTextMeshProテキストのフォントをAppFont SDFに変更するEditorスクリプト
/// </summary>
public class FontReplacer : EditorWindow
{
    [MenuItem("Tools/すべてのTMPフォントをAppFontに変更")]
    public static void ReplaceAllFonts()
    {
        // AppFont SDFをプロジェクトから検索
        string[] guids = AssetDatabase.FindAssets("AppFont SDF t:TMP_FontAsset");
        
        if (guids.Length == 0)
        {
            EditorUtility.DisplayDialog("エラー", "AppFont SDFが見つかりません。", "OK");
            return;
        }

        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        TMP_FontAsset appFont = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(path);

        if (appFont == null)
        {
            EditorUtility.DisplayDialog("エラー", "AppFont SDFのロードに失敗しました。", "OK");
            return;
        }

        // シーン内のすべてのTMP_Textを取得（非アクティブなオブジェクトも含む）
        TMP_Text[] allTexts = Resources.FindObjectsOfTypeAll<TMP_Text>();
        
        int changedCount = 0;
        foreach (TMP_Text text in allTexts)
        {
            // シーン内のオブジェクトのみ処理（プレハブは除外）
            if (text.gameObject.scene.isLoaded)
            {
                Undo.RecordObject(text, "フォント変更");
                text.font = appFont;
                EditorUtility.SetDirty(text);
                changedCount++;
            }
        }

        EditorUtility.DisplayDialog(
            "完了", 
            $"シーン内の {changedCount} 個のテキストのフォントをAppFont SDFに変更しました。\n\nシーンを保存してください。", 
            "OK"
        );
        
        Debug.Log($"[FontReplacer] {changedCount} 個のTMP_Textのフォントを AppFont SDF に変更しました。");
    }
}
