using UnityEngine;
using UnityEngine.UI;

public class BoxUI : MonoBehaviour
{
    [SerializeField] private ContainerBase _container;
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _gridParent;
    [SerializeField] private GameObject _gridPrefab;

    public void Start()
    {
        if (_container == null)
        {
            _container = GetComponent<ContainerBase>();
        }

        if (_panel == null)
        {
            _panel = transform.Find("Panel").gameObject;
        }

        if (_gridParent == null)
        {
            _gridParent = _panel.transform.Find("GridParent").gameObject;
        }

        _panel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            _panel.SetActive(!_panel.activeSelf);
            UpdateGrids();
        }
    }

    public void Show()
    {

    }

    public void UpdateGrids()
    {
        if (_gridPrefab == null) return;
        foreach (Transform child in _gridParent.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var item in _container.Slots)
        {
            if (item == null) continue;
            GameObject grid = Instantiate(_gridPrefab, _gridParent.transform);
            var gridUI = grid.GetComponent<Image>();
            if (gridUI != null)
            {
                gridUI.sprite = item.Icon;
            }
        }
    }

    public void AddAllButton()
    {
        PlayerBag.Instance.TransferAll(_container);
        UpdateGrids();
        _container.DebugPrint();
        PlayerBag.Instance.DebugPrint();
    }

    public void DumpAllButton()
    {
        _container.TransferAll(PlayerBag.Instance);
        UpdateGrids();
        _container.DebugPrint();
        PlayerBag.Instance.DebugPrint();
    }
}
