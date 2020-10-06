using UnityEngine;

public class SharkSpawner : MonoBehaviour
{
    [SerializeField] private GameObject sharkPrefab;

    private void Start()
    {
        var shark = Instantiate(sharkPrefab, Vector3.zero, Quaternion.identity);
        shark.transform.position = new Vector3(0, -1f, 0);
        shark.transform.rotation = Quaternion.Euler(0, 90, 0);
    }
}