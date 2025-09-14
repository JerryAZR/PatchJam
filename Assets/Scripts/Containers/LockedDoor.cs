using UnityEngine;

public class LockedDoor : ContainerBase
{
    public bool Open { get; private set; } = false;
    [SerializeField] private ItemBaseSO _key;
    [SerializeField] private Animator _unlockAnimator;

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
            }

        }
    }
}
