using MoreMountains.CorgiEngine;
using UnityEngine;

[RequireComponent(typeof(ButtonActivated))]
[RequireComponent(typeof(LevelSelector))]
public class LevelEntrance : MonoBehaviour
{
    [SerializeField] private Sprite _unlockedLevel;
    [SerializeField] private Sprite _clearedLevel;

    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _newHint;
    private ButtonActivated _triggerZone;
    private LevelSelector _selector;

    void Start()
    {
        _triggerZone = GetComponent<ButtonActivated>();
        _selector = GetComponent<LevelSelector>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (ProgressManager.Instance.GetState(_selector.LevelName))
        {
            case LevelState.Ready:
                _renderer.sprite = _unlockedLevel;
                _newHint.SetActive(true);
                _triggerZone.Activable = true;
                break;
            case LevelState.Cleared:
                _renderer.sprite = _clearedLevel;
                _triggerZone.Activable = true;
                _newHint.SetActive(false);
                break;
        }
    }
}
