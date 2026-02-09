using UnityEngine;

public class WorldSettingsInitializer : MonoBehaviour
{
    [SerializeField] NoiseSettings defaultNoise;
    [SerializeField] Transform biomeHandler;
    [SerializeField] DomainWarping domainWarping;
    [SerializeField] World world;

    private void Awake()
    {
        WorldSettings.AddData(defaultNoise, biomeHandler, domainWarping, world);
        Destroy(this);
    }
}