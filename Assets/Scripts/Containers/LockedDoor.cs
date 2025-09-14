using System.Collections;
using UnityEngine;

public class LockedDoor : ContainerBase
{
    public bool Open { get; private set; } = false;
    [SerializeField] private ItemBaseSO _key;
    [SerializeField] private Animator _unlockAnimator;
    [SerializeField] private GameObject _containerUI;

    // Update is called once per frame
    void Update()
    {
        if (Open) return;
        foreach (ItemBaseSO item in ViewSlots)
        {
            if (item == _key)
            {
                Open = true;
                _unlockAnimator.SetTrigger("Unlock");
                if (_containerUI != null) _containerUI.SetActive(false);
                StartCoroutine(UnlockCoroutine());
                break;
            }
        }
    }

    IEnumerator UnlockCoroutine()
    {
        yield return new WaitForSeconds(1.2f);
        this.gameObject.SetActive(false);
    }
}
