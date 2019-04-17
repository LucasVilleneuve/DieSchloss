using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject iconPrefab = null;
    [SerializeField] Transform contentPane = null;

    private bool pressingShowInput = false;
    private Canvas parentCanvas;

    private Dictionary<int, GameObject> items = new Dictionary<int, GameObject>();

    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        pressingShowInput = Input.GetButton("Show Inventory");

        parentCanvas.enabled = pressingShowInput;
    }

    public void AddItem(UsableObject item)
    {
        GameObject icon = Instantiate(iconPrefab, contentPane, false);
        foreach (Image image in icon.transform.GetComponentsInChildren<Image>())
        {
            if (image.gameObject != icon)
                image.sprite = item.sprite;
        }
        items.Add(item.id, icon);
    }

    public void RemoveItem(UsableObject item)
    {
        if (!items.ContainsKey(item.id)) return;

        GameObject go = items[item.id];
        Destroy(go);
        items.Remove(item.id);
    }
}
