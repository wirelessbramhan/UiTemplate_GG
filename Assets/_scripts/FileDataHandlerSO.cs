using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "word";
    private readonly string backupExtension = ".bak";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public ViewTemplateData Load(string profileId, bool allowRestoreFromBackup = true)
    {
        // base case - if the profileId is null, return right away
        if (profileId == null)
        {
            return null;
        }

        // used Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        ViewTemplateData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                // loads the serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // optionally decrypts the data
                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                // deserializes the data from Json back into the C# object
                loadedData = JsonUtility.FromJson<ViewTemplateData>(dataToLoad);
            }
            catch (Exception e)
            {
                //since Load is called recursively, this check ensures escape out of inifinte recursion loop
                if (allowRestoreFromBackup)
                {
                    Debug.LogWarning("Failed to load data file. Attempting to roll back.\n" + e);
                    bool rollbackSuccess = AttemptRollback(fullPath);
                    if (rollbackSuccess)
                    {
                        // tries to load again recursively
                        loadedData = Load(profileId, false);
                    }
                }
                
                // if else block hit, one possibility is that the backup file is also corrupt
                else
                {
                    Debug.LogError("Error occured when trying to load file at path: "
                        + fullPath + " and backup did not work.\n" + e);
                }
            }
        }
        return loadedData;
    }

    public void Save(ViewTemplateData data, string profileId)
    {
        // base case - if the profileId is null, returns right away
        if (profileId == null)
        {
            return;
        }

        // used Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        string backupFilePath = fullPath + backupExtension;
        try
        {
            // creates the directory the file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serializes the C# game data object into Json
            string dataToStore = JsonUtility.ToJson(data, true);

            // optionally encrypts the data (simple XOR)
            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            // writes the serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

            // verifies the newly saved file can be loaded successfully
            ViewTemplateData verifiedGameData = Load(profileId);

            // if the data can be verified, backs it up
            if (verifiedGameData != null)
            {
                File.Copy(fullPath, backupFilePath, true);
            }
            // otherwise, something went wrong and we should throw an exception
            else
            {
                throw new Exception("Save file could not be verified and backup could not be created.");
            }

        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public void Delete(string profileId)
    {
        // base case - if the profileId is null, return right away
        if (profileId == null)
        {
            return;
        }

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
        try
        {
            // ensures the data file exists at this path before deleting the directory
            if (File.Exists(fullPath))
            {
                // deletes the profile folder and everything within it
                Directory.Delete(Path.GetDirectoryName(fullPath), true);
            }
            else
            {
                Debug.LogWarning("Tried to delete profile data, but data was not found at path: " + fullPath);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to delete profile data for profileId: "
                + profileId + " at path: " + fullPath + "\n" + e);
        }
    }

    public Dictionary<string, ViewTemplateData> LoadAllProfiles()
    {
        Dictionary<string, ViewTemplateData> profileDictionary = new Dictionary<string, ViewTemplateData>();

        // loop over all directory names in the data directory path
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            // checks if the data file exists, else folder is skipped
            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory when loading all profiles because it does not contain data: "
                    + profileId);
                continue;
            }

            // loads the game data for this profile and put it in the dictionary
            ViewTemplateData profileData = Load(profileId);

            // defensive - ensures the profile data isn't null
            if (profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }
            else
            {
                Debug.LogError("Tried to load profile but something went wrong. ProfileId: " + profileId);
            }
        }

        return profileDictionary;
    }

    // the below is a simple implementation of XOR encryption
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }

    private bool AttemptRollback(string fullPath)
    {
        bool success = false;
        string backupFilePath = fullPath + backupExtension;
        try
        {
            // if the file exists, attempt to roll back to it by overwriting the original file
            if (File.Exists(backupFilePath))
            {
                File.Copy(backupFilePath, fullPath, true);
                success = true;
                Debug.LogWarning("Had to roll back to backup file at: " + backupFilePath);
            }
            // otherwise, we don't yet have a backup file - so there's nothing to roll back to
            else
            {
                throw new Exception("Tried to roll back, but no backup file exists to roll back to.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to roll back to backup file at: "
                + backupFilePath + "\n" + e);
        }

        return success;
    }
}

[CreateAssetMenu]
public class FileDataHandlerSO : ScriptableObject
{
    public FileDataHandler dataHandler;
    public static string currentFileName;

    public static event Action OnSaveEvent;
    public static event Action OnLoadEvent;
    public static int count = 0;
    public static ViewTemplateData currentData;
    private void OnEnable()
    {
        //Creates FileHandler class for reading/writind data streams
        dataHandler = new(Application.persistentDataPath, currentFileName, false);
        OnSaveEvent += SaveTemplateData;
        OnLoadEvent += LoadTemplateData;
    }

    private void OnDisable()
    {
        OnSaveEvent += SaveTemplateData;
        OnLoadEvent += LoadTemplateData;
    }

    public static void Save()
    {
        OnSaveEvent?.Invoke();
    }

    public static void Load()
    {
        OnLoadEvent?.Invoke();
    }

    private void SaveTemplateData()
    {
        dataHandler.Save(currentData, currentFileName);
    }

    private void LoadTemplateData()
    {
        dataHandler.Load(currentFileName);
    }

    //Sets attributes of savefile before Saving
    public static void SetSaveData(ViewTemplateData data, string filename)
    {
        currentData = data;
        currentFileName = filename + count + ".templatedata";
        Debug.Log(filename + "\n" + currentData.name);
    }
}
