using UnityEngine;

[CreateAssetMenu(fileName = "ItemBaseSO", menuName = "Scriptable Objects/ItemBaseSO")]
public class ItemBaseSO : ScriptableObject
{
    public string Name;
    [Min(0)] public float Weight;
    public Sprite Icon;
}
