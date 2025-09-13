using UnityEngine;

public class BoxUI : MonoBehaviour
{
    [SerializeField] private ContainerBase _container;
    [SerializeField] private GameObject _panel;

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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            _panel.SetActive(!_panel.activeSelf);
        }
    }

    public void Show()
    {

    }

    public void AddAllButton()
    {
        PlayerBag.Instance.TransferAll(_container);
    }

    public void DumpAllButton()
    {
        _container.TransferAll(PlayerBag.Instance);
    }
}
