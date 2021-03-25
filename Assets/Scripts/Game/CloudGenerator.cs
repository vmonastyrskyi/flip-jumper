using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class CloudGenerator : MonoBehaviour
    {
        private const int InitialCloudCount = 8;
        private const int DistanceBetweenClouds = 25;
        private const float CloudSpeed = 2;
        private const float InitialCloudDistance = 180;
        private const int DestroyCloudDistance = 40;

        [SerializeField] private GameObject[] cloudPrefabs;

        private Camera _mainCamera;
        private Transform _cameraTransform;

        private readonly List<GameObject> _clouds = new List<GameObject>();

        private void Start()
        {
            _mainCamera = Camera.main;
            if (_mainCamera != null)
            {
                _cameraTransform = _mainCamera.transform;
                SpawnClouds();
            }
        }

        private void Update()
        {
            GenerateClouds();
            MoveClouds();
        }

        private void SpawnClouds()
        {
            for (var i = 0; i < InitialCloudCount; i++)
                _clouds.Add(CreateCloud(DistanceBetweenClouds * i));
        }

        private void MoveClouds()
        {
            foreach (var cloud in _clouds)
            {
                var position = cloud.transform.position;
                cloud.transform.position =
                    new Vector3(position.x - (CloudSpeed * Time.unscaledDeltaTime), position.y, position.z);

                if ((_cameraTransform.position.x - cloud.transform.position.x) > DestroyCloudDistance)
                {
                    Destroy(cloud);
                    _clouds.Remove(cloud);
                    break;
                }
            }
        }

        private void GenerateClouds()
        {
            var lastCloud = _clouds.Last();
            if ((_cameraTransform.position.x + InitialCloudDistance - lastCloud.transform.position.x) >
                DistanceBetweenClouds)
                _clouds.Add(CreateCloud(InitialCloudDistance));
        }

        private GameObject CreateCloud(float distance)
        {
            var randomScale = Random.value + 1.5f;
            var randomCloudIndex = Random.Range(0, cloudPrefabs.Length);
            var cameraTransformPosition = _cameraTransform.position;

            var cloud = Instantiate(
                cloudPrefabs[randomCloudIndex],
                new Vector3(
                    cameraTransformPosition.x + distance,
                    Random.Range(-20, -10),
                    cameraTransformPosition.z + Random.Range(10, 30)),
                Quaternion.identity);

            cloud.transform.parent = _cameraTransform;
            cloud.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            return cloud;
        }
    }
}