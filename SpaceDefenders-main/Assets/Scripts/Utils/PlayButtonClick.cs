using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlayButtonClick : MonoBehaviour, IPointerEnterHandler
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayButtonClick();
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlayButtonHover();
    }
}
