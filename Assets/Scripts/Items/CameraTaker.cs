using MoreMountains.CorgiEngine;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CameraTaker : MonoBehaviour
{
    public void TakeCamera()
    {
        Character target = GetComponent<Character>();
        MMCameraEvent.Trigger(MMCameraEventTypes.SetTargetCharacter, target);
		MMCameraEvent.Trigger(MMCameraEventTypes.StartFollowing);
    }
}
