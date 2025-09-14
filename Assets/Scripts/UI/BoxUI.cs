using System.Collections.Generic;
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

    [SerializeField] private Button _addAllButton;
    [SerializeField] private Button _dumpAllButton;

    [SerializeField] private Image _indexIcon;

    [SerializeField] private List<Sprite> _orderSprites;

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
            SetInitialState();
            //_panel.SetActive(true);
            UpdateGrids();
        }

        ContainerBase.OnDataTransfer += UpdateGrids;
        StorageManager.OnInit += UpdateIndex;
    }

    void UpdateIndex()
    {
        if (_container != null)
        {
            _indexIcon.sprite = _orderSprites[_container.Order - 1];
        }
    }

    void SetInitialState()
    {
        float width = 560 * _container.Capacity / 8;
        _panel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(width, 80);
    }

    void OnDestroy()
    {
        ContainerBase.OnDataTransfer -= UpdateGrids;
        StorageManager.OnInit -= UpdateIndex;
    }

    void Update()
    {
        if (this.gameObject.name != "PlayerBag")
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _panel.SetActive(true);
                UpdateGrids();
            }

            if (Input.GetKeyUp(KeyCode.Tab))
            {
                _panel.SetActive(false);
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

            var gridUI = grid.transform.GetChild(0).GetComponent<Image>();
            if (gridUI != null)
            {
                gridUI.gameObject.SetActive(false);
                if (_container.Slots == null) continue;
                if (_container.Slots[i] == null)
                {
                    gridUI.sprite = null;
                    //gridUI.transform.GetChild(0).gameObject.SetActive(true);
                    continue;
                }
                gridUI.gameObject.SetActive(true);
                gridUI.sprite = _container.Slots[i].Icon;
                gridControl.so = _container.Slots[i];
                //gridUI.transform.GetChild(0).gameObject.SetActive(false);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (this.gameObject.name != "PlayerBag")
            {
                _panel.SetActive(true);
                if (_dumpAllButton != null) _dumpAllButton.gameObject.SetActive(true);
                if (_addAllButton != null) _addAllButton.gameObject.SetActive(true);
                UpdateGrids();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (this.gameObject.name != "PlayerBag")
            {
                _panel.SetActive(false);
                if (_dumpAllButton != null) _dumpAllButton.gameObject.SetActive(false);
                if (_dumpAllButton != null) _addAllButton.gameObject.SetActive(false);
            }
        }
    }
}
