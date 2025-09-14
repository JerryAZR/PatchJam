using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxUI : MonoBehaviour
{
    [SerializeField] private ContainerBase _container;
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _gridParent;
    [SerializeField] private GameObject _gridPrefab;

    [SerializeField] private TMP_Text _countText;
    [SerializeField] private TMP_Text _weightText;

    private int _count;
    private int _weight;

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

        if (this.gameObject.name != "PlayerBag")
            _panel.SetActive(false);
        else
        {
            //_panel.SetActive(true);
            UpdateGrids();
        }

        ContainerBase.OnDataTransfer += UpdateGrids;
    }

    void OnDestroy()
    {
        ContainerBase.OnDataTransfer -= UpdateGrids;
    }

    void Update()
    {
        if (this.gameObject.name != "PlayerBag")
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                _panel.SetActive(!_panel.activeSelf);
                UpdateGrids();
            }
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
        for (var i = 0; i < _container.Capacity; i++)
        {
            GameObject grid = Instantiate(_gridPrefab, _gridParent.transform);

            var gridControl = grid.GetComponent<GridUI>();
            if (gridControl != null)
            {
                gridControl.containerBase = _container;
                gridControl.boxUI = this;
                gridControl.index = i;
            }

            var gridUI = grid.GetComponent<Image>();
            if (gridUI != null)
            {
                if (_container.Slots == null) continue;
                if (_container.Slots[i] == null)
                {
                    gridUI.sprite = null;
                    gridUI.transform.GetChild(0).gameObject.SetActive(true);
                    continue;
                }
                gridUI.sprite = _container.Slots[i].Icon;
                gridControl.so = _container.Slots[i];
                gridUI.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        // foreach (var item in _container.Slots)
        // {
        //     if (item == null) continue;
        //     GameObject grid = Instantiate(_gridPrefab, _gridParent.transform);
        //     var gridUI = grid.GetComponent<Image>();
        //     if (gridUI != null)
        //     {
        //         gridUI.sprite = item.Icon;
        //     }
        // }
        if (_countText != null)
            UpdateCount();
        if (_weightText != null)
            UpdateWeight();
    }

    private void UpdateCount()
    {
        _count = _container.ItemCount;
        _countText.text = $"Count: {_count}/{_container.Capacity}";
    }

    private void UpdateWeight()
    {
        _weight = 0;
        _weightText.text = $"Weight: {_weight}";
    }

    public void AddAllButton()
    {
        PlayerBag.Instance.TransferAll(_container);
        // UpdateGrids();
        // PlayerBag.Instance.GetComponent<BoxUI>()?.UpdateGrids();
        _container.DebugPrint();
        PlayerBag.Instance.DebugPrint();
    }

    public void DumpAllButton()
    {
        _container.TransferAll(PlayerBag.Instance);
        // UpdateGrids();
        // PlayerBag.Instance.GetComponent<BoxUI>()?.UpdateGrids();
        _container.DebugPrint();
        PlayerBag.Instance.DebugPrint();
    }
}
