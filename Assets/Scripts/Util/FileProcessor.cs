using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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


    public static List<string> FetchFilePaths(List<string> allFiles, List<string> targetFileNames)
    {
        return allFiles.Where(file => targetFileNames.Contains(Path.GetFileName(file))).ToList();
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


    public static List<string> GetSortedImageFilesForMatchingSides(string directoryPath, string prefix = "")
    {
        if (Directory.Exists(directoryPath))
        {
            return Directory.GetFiles(directoryPath)
                            .Where(file => ImageExtensions.Contains(Path.GetExtension(file).ToLower()) &&
                                           (string.IsNullOrEmpty(prefix) || Path.GetFileName(file).StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
                            .OrderBy(file => ExtractImageNumber(file))
                            .ThenBy(file => ExtractImageSide(file))
                            .ToList();
        }
        else
        {
            return new List<string>();
        }
    }

    private static int ExtractImageNumber(string fileName)
    {
        // Split by '_'
        string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        var parts = nameWithoutExtension.Split('_');

        if (parts.Length > 1)
        {
            // Remove non-digit characters from the first part to get the number
            string numberPart = new string(parts[0].Where(char.IsDigit).ToArray());

            Debug.Log($"Extracted number part: {numberPart} from file: {fileName}");
            return int.TryParse(numberPart, out int number) ? number : int.MaxValue;
        }

        return int.MaxValue;
    }

    private static string ExtractImageSide(string fileName)
    {
        // Split by '_'
        string nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        var parts = nameWithoutExtension.Split('_');

        if (parts.Length > 1)
        {
            // Take the second part as the side
            string sidePart = parts[1].ToLower();
            Debug.Log($"Extracted side part: {sidePart} from file: {fileName}");
            return sidePart;
        }

        return string.Empty;
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

    public static Sprite GetSpriteByFileName(string filePath, int targetCells = 2)
    {
        // string filePath = Path.Combine(directoryPath, fileName);
        if (File.Exists(filePath) && ImageExtensions.Contains(Path.GetExtension(filePath).ToLower()))
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(1, 1);
            if (texture.LoadImage(fileData))
            {
                // Calculate target dimensions based on cell size
                int targetWidth = 100 * targetCells; // 3 cells width
                int targetHeight = Mathf.RoundToInt((float)texture.height / texture.width * targetWidth);

                // Resize the texture while preserving aspect ratio
                Texture2D resizedTexture = ResizeTexture(texture, targetWidth, targetHeight);

                return Sprite.Create(resizedTexture, new Rect(0, 0, resizedTexture.width, resizedTexture.height), new Vector2(0.5f, 0.5f), 100f);
            }
        }
        MyDebug.LogWarning($"Image file not found or invalid: {filePath}");
        return null;
    }

    private static Texture2D ResizeTexture(Texture2D originalTexture, int targetWidth, int targetHeight)
    {
        // Calculate new dimensions while preserving aspect ratio
        float aspectRatio = (float)originalTexture.width / originalTexture.height;
        int newWidth = Mathf.RoundToInt(targetHeight * aspectRatio);
        int newHeight = Mathf.RoundToInt(targetWidth / aspectRatio);

        // Use the smaller dimension for resizing
        if (newWidth > targetWidth)
        {
            newWidth = targetWidth;
        }
        else
        {
            newHeight = targetHeight;
        }

        // Resize the texture
        RenderTexture renderTexture = RenderTexture.GetTemporary(newWidth, newHeight);
        RenderTexture.active = renderTexture;
        Graphics.Blit(originalTexture, renderTexture);
        Texture2D resizedTexture = new Texture2D(newWidth, newHeight);
        resizedTexture.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        resizedTexture.Apply();
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(renderTexture);
        return resizedTexture;
    }

    public static void GetAudioClipByFileName(string filePath, Action<AudioClip> onComplete)
    {
        if (File.Exists(filePath) && AudioExtensions.Contains(Path.GetExtension(filePath).ToLower()))
        {
            AudioLoader audioLoader = new GameObject("AudioLoader").AddComponent<AudioLoader>();
            MyDebug.Log($"FILE NAME: {filePath}");
            audioLoader.LoadAudioClipCoroutine(filePath, onComplete);
        }
        else
        {
            MyDebug.LogWarning($"Audio file not found or invalid: {filePath}");
            onComplete?.Invoke(null);
        }
    }

    #region Json File Handler

    protected string encryptionKey = "alpha-gamma"; // You should use a securely generated key
    public string userFilePath = Application.persistentDataPath + "/ibuiuhad.ji"; // You should use a securely generated key

    public void SaveJsonToFile(string filePath, string jsonString)
    {
        try
        {
            string encryptedJson = EncryptString(jsonString, encryptionKey);
            File.WriteAllText(filePath, encryptedJson);
            Debug.Log("JSON saved and encrypted to file successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to save JSON to file: " + ex.Message);
        }
    }

    public string LoadJsonFromFile(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                string encryptedJson = File.ReadAllText(filePath);
                string decryptedJson = DecryptString(encryptedJson, encryptionKey);
                Debug.Log("JSON loaded and decrypted from file successfully.");
                return decryptedJson;
            }
            else
            {
                Debug.LogWarning("JSON file not found.");
                return null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to load JSON from file: " + ex.Message);
            return null;
        }
    }

    private string EncryptString(string plainText, string key)
    {
        using (Aes aesAlg = Aes.Create())
        {
            using (Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes(key, Encoding.UTF8.GetBytes("your-salt")))
            {
                aesAlg.Key = keyGenerator.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = keyGenerator.GetBytes(aesAlg.BlockSize / 8);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }
    }

    private string DecryptString(string cipherText, string key)
    {
        using (Aes aesAlg = Aes.Create())
        {
            using (Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes(key, Encoding.UTF8.GetBytes("your-salt")))
            {
                aesAlg.Key = keyGenerator.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = keyGenerator.GetBytes(aesAlg.BlockSize / 8);

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

    }
    #endregion

}
