using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public Button playButton;
    public Button quitButton;
    public Button muteButton;
    public GameObject lobbyGameObject;

    private void Awake()
    {
        playButton.onClick.AddListener(PlayGame);
        quitButton.onClick.AddListener(QuitGame);
        muteButton.onClick.AddListener(MuteGame);
    }
    private void MuteGame()
    {
        TextMeshProUGUI muteButtonText = muteButton.GetComponentInChildren<TextMeshProUGUI>();
        if (muteButtonText.text == "Mute: On")
        {
            muteButtonText.text = "Mute: Off";
        }
        else {
            muteButtonText.text = "Mute: On";
        }
        SoundManager.Instance.MuteGame();
        SoundManager.Instance.PlayEffect(SoundType.ButtonClick);
    }
    private void PlayGame()
    {
        SoundManager.Instance.PlayEffect(SoundType.ButtonClick);
        lobbyGameObject.SetActive(true);
    }
    private void QuitGame()
    {
        SceneManagerUtility.QuitGame();
    }
}
