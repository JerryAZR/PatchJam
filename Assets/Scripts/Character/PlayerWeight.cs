using UnityEngine;

public class PlayerWeight : MonoBehaviour, IWeighted
{
    public float Weight => PlayerBag.Instance.Weight;
}
