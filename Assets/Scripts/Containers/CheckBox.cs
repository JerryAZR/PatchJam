using UnityEngine;

/// <summary>
/// Becomes "Checked" when one of the items within the assigned range
/// matches the
/// </summary>
public class CheckBox : ContainerBase
{
    public bool Checked { get; private set; }
    [SerializeField] private ItemBaseSO _expected;

    // Update is called once per frame
    void Update()
    {
        if (Checked) return;
        foreach (ItemBaseSO item in _slots)
        {
            if (item== _expected)
            {
                Checked = true;

            }

        }
    }
}
