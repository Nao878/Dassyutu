using UnityEngine;
using UnityEditor;
using TMPro;
using System.IO;

/// <summary>
/// TextMeshProフォント関連の問題を解決するEditorツール
/// </summary>
public class FontReplacer : EditorWindow
{
    private static readonly string RESOURCES_FONTS_PATH = "Assets/Resources/Fonts";
    private static readonly string FONT_ASSET_NAME = "AppFont SDF";

    [MenuItem("Tools/TMPフォント設定/1. すべてのTMPフォントをAppFontに変更")]
    public static void ReplaceAllFonts()
    {
        TMP_FontAsset appFont = FindAppFont();
        if (appFont == null) return;

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

    [MenuItem("Tools/TMPフォント設定/2. フォントをResourcesにコピー（ビルド用）")]
    public static void CopyFontToResources()
    {
        // AppFont SDFを検索
        string[] guids = AssetDatabase.FindAssets("AppFont SDF t:TMP_FontAsset");
        
        if (guids.Length == 0)
        {
            EditorUtility.DisplayDialog("エラー", "AppFont SDFが見つかりません。", "OK");
            return;
        }

        string sourcePath = AssetDatabase.GUIDToAssetPath(guids[0]);
        
        // Resourcesフォルダを作成
        if (!Directory.Exists(RESOURCES_FONTS_PATH))
        {
            Directory.CreateDirectory(RESOURCES_FONTS_PATH);
            AssetDatabase.Refresh();
        }

        string destPath = $"{RESOURCES_FONTS_PATH}/{FONT_ASSET_NAME}.asset";
        
        // 既に存在する場合は確認
        if (File.Exists(destPath))
        {
            if (!EditorUtility.DisplayDialog("確認", 
                "Resources/Fontsに既にAppFont SDFが存在します。上書きしますか？", 
                "上書き", "キャンセル"))
            {
                return;
            }
        }

        // コピー
        AssetDatabase.CopyAsset(sourcePath, destPath);
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog(
            "完了", 
            $"フォントをResourcesフォルダにコピーしました。\n\nパス: {destPath}\n\nこれでビルド時にフォントが含まれます。", 
            "OK"
        );
        
        Debug.Log($"[FontReplacer] フォントをResourcesにコピーしました: {destPath}");
    }

    [MenuItem("Tools/TMPフォント設定/3. TMP Settings のデフォルトフォントを設定")]
    public static void SetDefaultFont()
    {
        TMP_FontAsset appFont = FindAppFont();
        if (appFont == null) return;

        // TMP_Settingsを取得
        string[] settingsGuids = AssetDatabase.FindAssets("TMP Settings t:TMP_Settings");
        
        if (settingsGuids.Length == 0)
        {
            EditorUtility.DisplayDialog("エラー", 
                "TMP Settingsが見つかりません。\nTextMesh Proパッケージを確認してください。", "OK");
            return;
        }

        string settingsPath = AssetDatabase.GUIDToAssetPath(settingsGuids[0]);
        TMP_Settings tmpSettings = AssetDatabase.LoadAssetAtPath<TMP_Settings>(settingsPath);

        if (tmpSettings == null)
        {
            EditorUtility.DisplayDialog("エラー", "TMP Settingsのロードに失敗しました。", "OK");
            return;
        }

        // SerializedObjectを使用してデフォルトフォントを設定
        SerializedObject serializedSettings = new SerializedObject(tmpSettings);
        SerializedProperty defaultFontAssetProp = serializedSettings.FindProperty("m_defaultFontAsset");
        
        if (defaultFontAssetProp != null)
        {
            defaultFontAssetProp.objectReferenceValue = appFont;
            serializedSettings.ApplyModifiedProperties();
            EditorUtility.SetDirty(tmpSettings);
            AssetDatabase.SaveAssets();

            EditorUtility.DisplayDialog(
                "完了", 
                "TMP Settingsのデフォルトフォントを AppFont SDF に設定しました。\n\n新しく作成されるテキストは自動的にこのフォントを使用します。", 
                "OK"
            );
            
            Debug.Log("[FontReplacer] TMP Settingsのデフォルトフォントを設定しました。");
        }
        else
        {
            EditorUtility.DisplayDialog("エラー", "デフォルトフォントプロパティが見つかりません。", "OK");
        }
    }

    [MenuItem("Tools/TMPフォント設定/4. GameControllerにフォントを自動設定")]
    public static void SetGameControllerFont()
    {
        TMP_FontAsset appFont = FindAppFont();
        if (appFont == null) return;

        // シーン内のGameControllerを検索
        GameController[] controllers = Resources.FindObjectsOfTypeAll<GameController>();
        
        int count = 0;
        foreach (var controller in controllers)
        {
            if (controller.gameObject.scene.isLoaded)
            {
                Undo.RecordObject(controller, "GameControllerフォント設定");
                controller.appFont = appFont;
                EditorUtility.SetDirty(controller);
                count++;
            }
        }

        if (count > 0)
        {
            EditorUtility.DisplayDialog(
                "完了", 
                $"{count}個のGameControllerにAppFont SDFを設定しました。\n\nシーンを保存してください。", 
                "OK"
            );
            Debug.Log($"[FontReplacer] {count}個のGameControllerにフォントを設定しました。");
        }
        else
        {
            EditorUtility.DisplayDialog("情報", 
                "シーン内にGameControllerが見つかりませんでした。\nGameBootstrapが起動時にGameControllerを生成するため、Inspectorで手動設定が必要です。", 
                "OK"
            );
        }
    }

    [MenuItem("Tools/TMPフォント設定/5. すべての問題を一括修正")]
    public static void FixAllIssues()
    {
        EditorUtility.DisplayProgressBar("フォント問題を修正中...", "フォントをResourcesにコピー中...", 0.25f);
        CopyFontToResourcesSilent();

        EditorUtility.DisplayProgressBar("フォント問題を修正中...", "デフォルトフォントを設定中...", 0.5f);
        SetDefaultFontSilent();

        EditorUtility.DisplayProgressBar("フォント問題を修正中...", "シーン内のテキストを更新中...", 0.75f);
        ReplaceAllFontsSilent();

        EditorUtility.DisplayProgressBar("フォント問題を修正中...", "GameControllerを設定中...", 0.9f);
        SetGameControllerFontSilent();

        EditorUtility.ClearProgressBar();

        EditorUtility.DisplayDialog(
            "すべての修正が完了しました", 
            "以下の修正を行いました:\n\n" +
            "✅ フォントをResources/Fontsにコピー\n" +
            "✅ TMP Settingsのデフォルトフォントを設定\n" +
            "✅ シーン内のすべてのTMPテキストのフォントを変更\n" +
            "✅ GameControllerにフォントを設定\n\n" +
            "⚠️ シーンを保存してください (Ctrl+S)\n" +
            "⚠️ ビルド後も日本語が正常に表示されます", 
            "OK"
        );
    }

    // ヘルパーメソッド
    private static TMP_FontAsset FindAppFont()
    {
        // まずResourcesフォルダを確認
        string resourcesPath = $"{RESOURCES_FONTS_PATH}/{FONT_ASSET_NAME}.asset";
        TMP_FontAsset appFont = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(resourcesPath);
        
        if (appFont != null) return appFont;

        // Resourcesになければ他の場所を検索
        string[] guids = AssetDatabase.FindAssets("AppFont SDF t:TMP_FontAsset");
        
        if (guids.Length == 0)
        {
            EditorUtility.DisplayDialog("エラー", "AppFont SDFが見つかりません。", "OK");
            return null;
        }

        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        appFont = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(path);

        if (appFont == null)
        {
            EditorUtility.DisplayDialog("エラー", "AppFont SDFのロードに失敗しました。", "OK");
            return null;
        }

        return appFont;
    }

    // サイレント版（ダイアログなし）
    private static void CopyFontToResourcesSilent()
    {
        string[] guids = AssetDatabase.FindAssets("AppFont SDF t:TMP_FontAsset");
        if (guids.Length == 0) return;

        string sourcePath = AssetDatabase.GUIDToAssetPath(guids[0]);
        
        if (!Directory.Exists(RESOURCES_FONTS_PATH))
        {
            Directory.CreateDirectory(RESOURCES_FONTS_PATH);
            AssetDatabase.Refresh();
        }

        string destPath = $"{RESOURCES_FONTS_PATH}/{FONT_ASSET_NAME}.asset";
        
        if (!sourcePath.Equals(destPath))
        {
            AssetDatabase.CopyAsset(sourcePath, destPath);
            AssetDatabase.Refresh();
        }
    }

    private static void SetDefaultFontSilent()
    {
        TMP_FontAsset appFont = FindAppFontSilent();
        if (appFont == null) return;

        string[] settingsGuids = AssetDatabase.FindAssets("TMP Settings t:TMP_Settings");
        if (settingsGuids.Length == 0) return;

        string settingsPath = AssetDatabase.GUIDToAssetPath(settingsGuids[0]);
        TMP_Settings tmpSettings = AssetDatabase.LoadAssetAtPath<TMP_Settings>(settingsPath);
        if (tmpSettings == null) return;

        SerializedObject serializedSettings = new SerializedObject(tmpSettings);
        SerializedProperty defaultFontAssetProp = serializedSettings.FindProperty("m_defaultFontAsset");
        
        if (defaultFontAssetProp != null)
        {
            defaultFontAssetProp.objectReferenceValue = appFont;
            serializedSettings.ApplyModifiedProperties();
            EditorUtility.SetDirty(tmpSettings);
            AssetDatabase.SaveAssets();
        }
    }

    private static void ReplaceAllFontsSilent()
    {
        TMP_FontAsset appFont = FindAppFontSilent();
        if (appFont == null) return;

        TMP_Text[] allTexts = Resources.FindObjectsOfTypeAll<TMP_Text>();
        
        foreach (TMP_Text text in allTexts)
        {
            if (text.gameObject.scene.isLoaded)
            {
                Undo.RecordObject(text, "フォント変更");
                text.font = appFont;
                EditorUtility.SetDirty(text);
            }
        }
    }

    private static void SetGameControllerFontSilent()
    {
        TMP_FontAsset appFont = FindAppFontSilent();
        if (appFont == null) return;

        GameController[] controllers = Resources.FindObjectsOfTypeAll<GameController>();
        
        foreach (var controller in controllers)
        {
            if (controller.gameObject.scene.isLoaded)
            {
                Undo.RecordObject(controller, "GameControllerフォント設定");
                controller.appFont = appFont;
                EditorUtility.SetDirty(controller);
            }
        }
    }

    private static TMP_FontAsset FindAppFontSilent()
    {
        string resourcesPath = $"{RESOURCES_FONTS_PATH}/{FONT_ASSET_NAME}.asset";
        TMP_FontAsset appFont = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(resourcesPath);
        
        if (appFont != null) return appFont;

        string[] guids = AssetDatabase.FindAssets("AppFont SDF t:TMP_FontAsset");
        if (guids.Length == 0) return null;

        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        return AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(path);
    }
}
