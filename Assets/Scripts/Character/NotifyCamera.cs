using MoreMountains.CorgiEngine;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class NotifyCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Character player = GetComponent<Character>();
        MMCameraEvent.Trigger(MMCameraEventTypes.SetTargetCharacter, player);
		MMCameraEvent.Trigger(MMCameraEventTypes.StartFollowing);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
