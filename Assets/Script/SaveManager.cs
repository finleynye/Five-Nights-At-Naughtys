using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public static class SaveManager
{
    public static readonly string SaveFolder = $"{Application.persistentDataPath}/GameData";
    
    public static SaveProfile<T> Load<T>(string profileName) where T : SaveProfileData
    {
        if (!File.Exists($"{SaveFolder}/{profileName}"))
            throw new Exception($"Save Profile {profileName} not found");
        
        if (!Directory.Exists(SaveFolder))
            Directory.CreateDirectory(SaveFolder);

        var encryptedContents = File.ReadAllText($"{SaveFolder}/{profileName}");
        var decodedBytes = Convert.FromBase64String(encryptedContents);
        var decryptedContents = Encoding.UTF8.GetString(decodedBytes);
        
        //Debug.Log($"Successfully loaded {SaveFolder}/{profileName}");
        return JsonConvert.DeserializeObject<SaveProfile<T>>(decryptedContents);
    }

    public static void Save<T>(SaveProfile<T> saveProfile) where T : SaveProfileData
    {
        if (File.Exists($"{SaveFolder}/{saveProfile.name}"))
            throw new Exception($"Save Profile {saveProfile.name} already exists");

        var jsonString = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(saveProfile, Formatting.Indented,
            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore })));

        if (!Directory.Exists(SaveFolder))
            Directory.CreateDirectory(SaveFolder); //create save folder path if doesnt exist
        File.WriteAllText($"{SaveFolder}/{saveProfile.name}", jsonString);
    }
    
    public static void SaveAs<T>(SaveProfile<T> saveProfile, bool overwriteData = false) where T : SaveProfileData
    {
        try
        {
            if (!overwriteData && File.Exists($"{SaveFolder}/{saveProfile.name}"))
                throw new Exception($"{saveProfile.name} already exists, use a different profile name");

            //encrypted
            var jsonString = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(saveProfile, Formatting.Indented,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore })));
            
            
            //Overwrite data to file
            if (!Directory.Exists(SaveFolder))
                Directory.CreateDirectory(SaveFolder);
            
            File.WriteAllText($"{SaveFolder}/{saveProfile.name}", jsonString);
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to save data into {saveProfile.name}. Full stack trace:\n{e}");
        }
    }

    public static void Delete(string profileName)
    {
        if (!File.Exists($"{SaveFolder}/{profileName}"))
            throw new Exception($"Save Profile {profileName} not found");
        
        File.Delete($"{SaveFolder}/{profileName}");
        Debug.Log($"Successfully deleted {SaveFolder}/{profileName}");
    }
    
}