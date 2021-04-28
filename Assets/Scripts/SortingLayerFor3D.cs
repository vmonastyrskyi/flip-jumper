using UnityEngine;

public class SortingLayerFor3D : MonoBehaviour
{
    [SerializeField] private string sortingLayer;
    [SerializeField] private int orderInLayer;

    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        _renderer.sortingLayerName = sortingLayer;
        _renderer.sortingOrder = orderInLayer;
    }
}