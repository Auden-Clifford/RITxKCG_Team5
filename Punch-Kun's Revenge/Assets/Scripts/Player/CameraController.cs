using Unity.Cinemachine;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private CinemachineCamera _mainCineCam;
    [SerializeField] private float _shakeIntensity = 2f;

    private CinemachineImpulseSource _impulseSource;

    public CinemachineCamera MainCineCam => _mainCineCam;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerTakeDamage += Shake;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerTakeDamage -= Shake;
    }

    private void Start()
    {
        if (!_mainCineCam.TryGetComponent(out CinemachineImpulseSource impulseSource))
        {
            Debug.LogError("Camera does not have an impulse source!");
            enabled = false;
            return;
        }

        _impulseSource = impulseSource;
    }

    public void Shake() => Shake(_shakeIntensity);

    public void Shake(float intensity = 2f) => _impulseSource.GenerateImpulse(intensity);
}