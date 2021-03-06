using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ModIO;
using UnityEngine;

public static class AvailableMods
{
    public static event Action onUpdated;

    static AvailableMods()
    {
        localMods = new ModData[] {};
        modIOMods = new ModData[] {};
    }

    public static void Refresh()
    {
        localMods = GetLocalMods();
        GetModIOMods();
        onUpdated?.Invoke();
    }

    static ModData[] GetLocalMods()
    {
        if (!Directory.Exists(ModData.localModPath)) return new ModData[] {};

        return Directory.GetDirectories(ModData.localModPath)
            .Select(path => {
                var dir = new DirectoryInfo(path);
                var guid = dir.Name;
                return ModData.GetLocal(guid);
            })
            .ToArray();
    }
    
    static void GetModIOMods()
    {
        var idPairs = new List<ModfileIdPair>();
        foreach (var kvp in ModManager.IterateInstalledMods(null)) idPairs.Add(kvp.Key);

        ModManager.GetModProfiles(
            idPairs.Select(_ => _.modId),
            (profiles) => {
                modIOMods = profiles
                    .GroupBy(_ => _.id)
                    .Select(_ => _.First())
                    .Select(_ => {
                        try
                        {
                            return ModData.FromModIOProfile(_);
                        }
                        catch (IOException)
                        {
                            return null;
                        }
                    })
                    .Where(_ => _ != null)
                    .ToArray();
                onUpdated?.Invoke();
            },
            (error) => ModIOController.DisplayWebError(error)
        );
    }

    public static IReadOnlyList<ModData> localMods { get; private set; }
    public static IReadOnlyList<ModData> modIOMods { get; private set; }
}

[Serializable]
public class ModData
{
    public const string DATA_FILE_NAME = "data.json";
    public const string LOGO_FILE_NAME = "preview.png";

    public static string localModPath =>
        Path.Combine(Application.persistentDataPath, "localMods");

    public static ModData NewLocal()
    {
        var guid = Guid.NewGuid().ToString();

        var data = new ModData();
        data.id = guid;
        data.name = guid;

        data.Save();

        return data;
    }

    public static ModData GetLocal(string guid)
    {
        var path = Path.Combine(localModPath, guid, DATA_FILE_NAME);

        var mod = FromPath(path);
        mod.source = Source.Local;

        return mod;
    }

    public static ModData FromModIOProfile(ModProfile profile)
    {
        var modFileID = profile.currentBuild.id;

        var dir = ModManager.GetModInstallDirectory(profile.id, modFileID);
        var path = Path.Combine(dir, DATA_FILE_NAME);

        var mod = FromPath(path);
        mod.id = profile.id.ToString();
        mod.source = Source.ModIO;
        mod.modFileID = modFileID;
        mod.modUserID = profile.submittedBy.id;

        return mod;
    }

    static ModData FromPath(string path)
    {
        var json = File.ReadAllText(path);
        var mod = JsonUtility.FromJson<ModData>(json);

        return mod;
    }

    // Control data
    public string id;
    public Source source;
    public string summary;

    public int modFileID;
    public int modUserID;

    public int modID => Int32.Parse(id);

    public string dirPath => source == Source.Local
        ? Path.Combine(localModPath, id)
        : ModManager.GetModInstallDirectory(modID, modFileID);
    
    public bool isFromCurrentUser => ModIOController.userExists &&
        modUserID == UserAuthenticationData.instance.userId;

    public void Save()
    {
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        var json = JsonUtility.ToJson(this);
        var dataPath = Path.Combine(dirPath, DATA_FILE_NAME);
        File.WriteAllText(dataPath, json);
    }

    public void Upload(Action successCB, Action failCB)
    {
        Action<WebRequestError> handleError = (error) => {
            ModIOController.DisplayWebError(error);
            failCB();
        };

        var profile = new EditableModProfile();
        profile.name.value = name;
        profile.name.isDirty = true;
        profile.summary.value = summary;
        profile.summary.isDirty = true;

        profile.logoLocator.value = new ImageLocatorData()
        {
            fileName = "Logo",
            url = Path.Combine(Application.streamingAssetsPath, LOGO_FILE_NAME)
        };
        profile.logoLocator.isDirty = true;

        profile.visibility.value = ModVisibility.Public;
        profile.visibility.isDirty = true;

        Action<ModProfile> postModSubmit = (modInfo) => {
            var releaseInfo = new EditableModfile();
            releaseInfo.version.value = "0.1.0";
            releaseInfo.version.isDirty = true;

            ModManager.UploadModBinaryDirectory(
                modInfo.id,
                releaseInfo,
                dirPath,
                true,
                (modFile) => {
                    if (source == Source.Local)
                    {
                        var newPath = ModManager.GetModInstallDirectory(modInfo.id, modFile.id);

                        var parent = new DirectoryInfo(newPath).Parent.Name;
                        if (!Directory.Exists(parent))
                            Directory.CreateDirectory(new DirectoryInfo(newPath).Parent.Name);

                        Directory.Move(dirPath, newPath);

                        var subs = ModManager.GetSubscribedModIds();
                        subs.Add(modInfo.id);
                        ModManager.SetSubscribedModIds(subs);
                        APIClient.SubscribeToMod(modInfo.id, null, null);

                        var enabled = ModManager.GetEnabledModIds();
                        enabled.Add(modInfo.id);
                        ModManager.SetEnabledModIds(enabled);

                        id = modInfo.id.ToString();
                        source = Source.ModIO;
                        modUserID = UserAuthenticationData.instance.userId;
                    }

                    modFileID = modFile.id;
                    Save();

                    successCB();

                    ModIOController.UpdateInstalledMods();
                },
                handleError);
        };
        
        if (source == Source.Local)
            ModManager.SubmitNewMod(profile, postModSubmit, handleError);
        else
            ModManager.SubmitModChanges(modID, profile, postModSubmit, handleError);
    }

    public void DeleteLocal()
    {
        var rootDir = localModPath;
        var dir = Path.Combine(rootDir, id);
        Directory.Delete(dir, true);

        if (Global.modData != null && Global.modData.id == id) Global.modData = null;
    }

    public enum Source
    {
        Local,
        ModIO
    }

    // Game data
    public string name;
    public List<LevelData> levels = new List<LevelData>();
}