using UnityEngine;

public class ReactorBehaviour : MonoBehaviour
{
    public GameObject Vehicle;
    private Color _firstColor, _startColor, _midColor, _lastColor;
    private float _maxEr;
    private int _maxP;
    private ParticleSystem.Particle[] _particleList;
    private ParticleSystem _syst;
    private VehicleMotor _vehicleMotor;
    // Use this for initialization
    private void Start()
    {
        _vehicleMotor = Vehicle.GetComponent<VehicleMotor>();
        _vehicleMotor.StateChanged += DrivingStateChanged;
        _syst = GetComponent<ParticleSystem>();
        _particleList = new ParticleSystem.Particle[_syst.particleCount];
        _syst.GetParticles(_particleList);
        _firstColor = new Color(206 / 255f, 27 / 255f, 27 / 255f, 1);
        _startColor = new Color(197 / 255f, 233 / 255f, 32 / 255f, 1);
        _midColor = new Color(188 / 255f, 6 / 255f, 32 / 255f, 1);
        _lastColor = new Color(2 / 255f, 2 / 255f, 2 / 255f, 1);
        _maxP = _syst.maxParticles;
        _maxEr = _syst.emissionRate;
    }

    private void DrivingStateChanged(object sender, VehicleMotor.StateChangedEventArgs e)
    {
        if (_syst != null)
        {
            if (e.State.Forward > 0)
            {
                _syst.Play();
            }
            else
            {
                _syst.Stop();
            }
            _particleList = new ParticleSystem.Particle[_syst.particleCount];
            _syst.GetParticles(_particleList);
            if (_vehicleMotor.IsBoosted)
            {
                _firstColor = new Color(27 / 255f, 27 / 255f, 206 / 255f, 1);
                _startColor = new Color(32 / 255f, 216 / 255f, 233 / 255f, 1);
                _midColor = new Color(72 / 255f, 4 / 255f, 251 / 255f, 1);
                _syst.startColor = _firstColor;
            }
            else
            {
                _firstColor = new Color(206 / 255f, 27 / 255f, 27 / 255f, 1);
                _startColor = new Color(197 / 255f, 233 / 255f, 32 / 255f, 1);
                _midColor = new Color(188 / 255f, 6 / 255f, 32 / 255f, 1);
                _syst.startColor = _firstColor;
            }
        }
    }
}