using System.Linq;
using ModIO;
using ModIO.UI;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class ModIOController : MonoBehaviour
{
    static ModIOController _i;

    public static bool userExists => !string.IsNullOrEmpty(UserAuthenticationData.instance.token);
    public static void ShowLogin() => _i._loginDialog.SetActive(true);
    public static void ShowMain()
    {
        _i._blur.SetActive(true);
        _i._explorerView.gameObject.SetActive(true);
        _i._footer.SetActive(true);

        if (_i._hasOpened) 
        {
            _i._explorerView.Refresh();
            _i._subscriptionsView.Refresh();
        } else _i._hasOpened = true;
    }

    public static void HideMain()
    {
        foreach (var obj in _i._toDisable) obj.SetActive(false);
    }

    public static void DisplayWebError(WebRequestError error) =>
        MessageSystem.QueueMessage(
            MessageDisplayData.Type.Error,
            error.displayMessage
        );

    public static void UpdateInstalledMods() =>
        _i.StartCoroutine(ModManager.DownloadAndUpdateMods_Coroutine(
            ModManager.GetInstalledModVersions(false)
                .Select(_ => _.modId)
                .ToArray(),
            null
        ));

#pragma warning disable CS0649
    [SerializeField] GameObject _blur;
    [SerializeField] ExplorerView _explorerView;
    [SerializeField] SubscriptionsView _subscriptionsView;
    [SerializeField] GameObject _footer;
    [SerializeField] GameObject _loginDialog;
    [SerializeField] GameObject[] _toDisable;
#pragma warning restore CS0649

    bool _hasOpened;

    void OnEnable() => _i = this;
    void OnDisable() => _i = null;

    void Start() => UpdateInstalledMods();
}
