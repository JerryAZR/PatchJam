using System.Linq;
using MoreMountains.CorgiEngine;
using UnityEngine;

[RequireComponent(typeof(LevelManager))]
public class RegisterEntryPoints : MonoBehaviour
{
    [SerializeField] private GameObject _levelRoot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_levelRoot == null) return;
        CheckPoint[] entryPoints = _levelRoot.GetComponentsInChildren<CheckPoint>();
        LevelManager manager = GetComponent<LevelManager>();
        manager.PointsOfEntry = entryPoints
            .Select(p => new PointOfEntry { Name = p.name, Position = p.transform })
            .ToList();
    }
}
