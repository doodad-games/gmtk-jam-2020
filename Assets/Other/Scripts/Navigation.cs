using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    public static void GoToEditor(ModData modData)
    {
        Editor.Prepare(modData);
        SceneManager.LoadScene("Editor");
    }

    public void ShowModIO() => ModIOController.ShowMain();
    public void GoToMenu() => SceneManager.LoadScene("Menu");
    public void GoToEditorSelect() => SceneManager.LoadScene("EditorSelect");
    public void GoToEditorNew()
    {
        Editor.Prepare(ModData.NewLocal());
        SceneManager.LoadScene("Editor");
    }
}
