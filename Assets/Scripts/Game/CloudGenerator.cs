using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Game
{
    public class CloudGenerator : MonoBehaviour
    {
        private const int InitialCloudCount = 8;
        private const int DistanceBetweenClouds = 25;
        private const float CloudSpeed = 1.5f;
        private const float InitialCloudDistance = 180;
        private const int DestroyCloudDistance = 40;
        
        public GameObject[] cloudPrefabs;
        public Transform Target { get; set; }

        private readonly List<GameObject> _clouds = new List<GameObject>();
        private readonly Random _random = new Random();

        private void Start()
        {
            SpawnClouds();
        }

        private void Update()
        {
            MoveClouds();

            GenerateCloud();
        }

        private void SpawnClouds()
        {
            for (var i = 0; i < InitialCloudCount; i++)
            {
                _clouds.Add(CreateCloud(DistanceBetweenClouds * i));
            }
        }

        private void MoveClouds()
        {
            foreach (var cloud in _clouds)
            {
                var position = cloud.transform.position;
                cloud.transform.position = new Vector3(position.x - (CloudSpeed * Time.deltaTime), position.y, position.z);

                if ((Target.position.x - cloud.transform.position.x) > DestroyCloudDistance)
                {
                    Destroy(cloud);
                    _clouds.Remove(cloud);
                    break;
                }
            }
        }

        private void GenerateCloud()
        {
            var lastCloud = _clouds.Last();
            if ((Target.position.x + InitialCloudDistance - lastCloud.transform.position.x) >
                DistanceBetweenClouds)
            {
                _clouds.Add(CreateCloud(InitialCloudDistance));
            }
        }

        private GameObject CreateCloud(float distance)
        {
            var randomScale = (float) (_random.NextDouble() + 1.5f);
            var randomCloudIndex = _random.Next(0, cloudPrefabs.Length);

            var cloud = Instantiate(
                cloudPrefabs[randomCloudIndex],
                new Vector3(
                    Target.position.x + distance,
                    _random.Next(-20, -10),
                    Target.position.z + _random.Next(-10, 20)),
                Quaternion.identity);
            cloud.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            return cloud;
        }
    }
}