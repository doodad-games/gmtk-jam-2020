using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    public static void GoToEditor(ModData modData)
    {
        Global.modData = modData;
        SceneManager.LoadScene("Editor");
    }
    public static void GoToEditLevel(LevelData level)
    {
        Global.SetCurrentLevelData(level, true);
        SceneManager.LoadScene(level.sceneKey);
    }
    public static void GoToPlayLevel(LevelData level)
    {
        Global.SetCurrentLevelData(level, false);
        SceneManager.LoadScene(level.sceneKey);
    }

    public void ShowModIO() => ModIOController.ShowMain();
    public void GoToMenu() => SceneManager.LoadScene("Menu");
    public void GoToEditorSelect() => SceneManager.LoadScene("EditorSelect");
    public void GoToEditorNew()
    {
        Global.modData = ModData.NewLocal();
        SceneManager.LoadScene("Editor");
    }
    public void GoToEditorCurrent() => SceneManager.LoadScene("Editor");
}
