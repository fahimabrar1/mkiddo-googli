using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class FileProcessor
{
    private static readonly string[] ImageExtensions = { ".jpg", ".jpeg", ".png" };
    private static readonly string[] AudioExtensions = { ".mp3", ".wav" };

    public static Dictionary<string, string> DNDTypes = new Dictionary<string, string>
    {
        { "learn_bangla_soroborno", "ban_sor_" },
        { "benjon_borno", "ban_banj_" },
        { "bangla_number", "ban_num_" },
        { "english_capital_letters", "en_" },
        { "english_small_letters", "en_" },
        { "english_number", "en_num_" },
        { "arabic_letters", "ar_" }
    };

    public static List<string> GetImageFiles(string directoryPath)
    {
        return Directory.Exists(directoryPath)
            ? Directory.GetFiles(directoryPath)
                      .Where(file => ImageExtensions.Contains(Path.GetExtension(file).ToLower()))
                      .ToList()
            : new List<string>();
    }

    public static List<string> GetAudioFiles(string directoryPath)
    {
        return Directory.Exists(directoryPath)
            ? Directory.GetFiles(directoryPath)
                      .Where(file => AudioExtensions.Contains(Path.GetExtension(file).ToLower()))
                      .ToList()
            : new List<string>();
    }

    public static List<string> GetSortedImageFiles(string directoryPath, string prefix = "")
    {
        if (Directory.Exists(directoryPath))
        {
            return Directory.GetFiles(directoryPath)
                            .Where(file => ImageExtensions.Contains(Path.GetExtension(file).ToLower()) &&
                                           (string.IsNullOrEmpty(prefix) || Path.GetFileName(file).StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
                            .OrderBy(file => ExtractId(file))
                            .ToList();
        }
        else
        {
            return new List<string>();
        }
    }

    public static List<string> GetSortedAudioFiles(string directoryPath, string prefix = "")
    {
        if (Directory.Exists(directoryPath))
        {
            return Directory.GetFiles(directoryPath)
                            .Where(file => AudioExtensions.Contains(Path.GetExtension(file).ToLower()) &&
                                           (string.IsNullOrEmpty(prefix) || Path.GetFileName(file).StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
                            .OrderBy(file => ExtractId(file))
                            .ToList();
        }
        else
        {
            return new List<string>();
        }
    }

    private static int ExtractId(string filePath)
    {
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        string[] parts = fileName.Split('_');
        if (parts.Length > 1 && int.TryParse(parts.Last(), out int id))
        {
            return id;
        }
        return int.MaxValue; // Assign a high value to files without a valid ID for sorting purposes
    }

    public static Sprite GetSpriteByFileName(string filePath)
    {
        // string filePath = Path.Combine(directoryPath, fileName);
        if (File.Exists(filePath) && ImageExtensions.Contains(Path.GetExtension(filePath).ToLower()))
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(1, 1);
            if (texture.LoadImage(fileData))
            {
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
            }
        }
        Debug.LogWarning($"Image file not found or invalid: {filePath}");
        return null;
    }

    public static void GetAudioClipByFileName(string filePath, Action<AudioClip> onComplete)
    {
        if (File.Exists(filePath) && AudioExtensions.Contains(Path.GetExtension(filePath).ToLower()))
        {
            AudioLoader audioLoader = new GameObject("AudioLoader").AddComponent<AudioLoader>();
            audioLoader.LoadAudioClipCoroutine(filePath, onComplete);
        }
        else
        {
            Debug.LogWarning($"Audio file not found or invalid: {filePath}");
            onComplete?.Invoke(null);
        }
    }


}
