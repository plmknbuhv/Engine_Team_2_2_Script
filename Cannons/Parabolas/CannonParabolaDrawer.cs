using EventSystem;
using Settings.InputSystem;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Cannons.Parabolas
{
    public class CannonParabolaDrawer : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO inputData;
        [SerializeField] private GameEventChannelSO cannonChannel;
        [SerializeField] private GameEventChannelSO parabolaChannel;
        [SerializeField] private ParabolaCalculator calculator;

        [Header("Viewer")] [SerializeField] private LineRenderer liner;
        [SerializeField] private DecalProjector endDecal;

        [Header("Color")] [SerializeField, ColorUsage(true, true)]
        private Color canShootColor;

        [SerializeField, ColorUsage(true, true)]
        private Color cantUseShootColor;

        [SerializeField] private string decalMatColorName = "_BlendColor";
        [SerializeField] private string linerMatColorName = "_LineColor";
        
        private Material _decalMaterial;
        private Material _linerMaterial;
        private int DecalColorHash;
        private int LinerColorHash;

        private bool _canShoot;
        private bool _canArrive;

        private void Awake()
        {
            DecalColorHash = Shader.PropertyToID(decalMatColorName);
            LinerColorHash = Shader.PropertyToID(linerMatColorName);
            _decalMaterial = endDecal.material;
            _linerMaterial = liner.material;
            
            cannonChannel.AddListener<CannonCheckCanShootEvent>(HandleCheckCanShoot);
            cannonChannel.AddListener<CannonChargingEvent>(HandleActiveParabola);
            parabolaChannel.AddListener<ParabolaChangeEvent>(HandleParabolaChanged);
        }

        private void Start()
        {
            SetViewerActive(false);
        }

        private void OnDestroy()
        {
            cannonChannel.RemoveListener<CannonCheckCanShootEvent>(HandleCheckCanShoot);
            cannonChannel.RemoveListener<CannonChargingEvent>(HandleActiveParabola);
            parabolaChannel.RemoveListener<ParabolaChangeEvent>(HandleParabolaChanged);
        }
        
        private void HandleParabolaChanged(ParabolaChangeEvent evt)
        {
            _canArrive = evt.canEnd;
            SetColor(_canShoot && _canArrive);
            
            var points = evt.points;
            liner.positionCount = points.Length;

            for (int i = 0; i < points.Length; i++)
            {
                liner.SetPosition(i, points[i]);
            }
            
            endDecal.transform.position = points[^1];
        }
        
        private void HandleActiveParabola(CannonChargingEvent evt)
        {
            SetViewerActive(evt.isCharging);
        }

        private void HandleCheckCanShoot(CannonCheckCanShootEvent evt)
        {
            _canShoot = evt.canShoot;
            SetColor(_canShoot && _canArrive);
        }

        private void SetViewerActive(bool isActive)
        {
            liner.gameObject.SetActive(isActive);
            endDecal.gameObject.SetActive(isActive);
        }
        
        private void SetColor(bool canShoot)
        {
            _linerMaterial.SetColor(LinerColorHash, canShoot ? canShootColor : cantUseShootColor);
            _decalMaterial.SetColor(DecalColorHash, canShoot ? canShootColor : cantUseShootColor);
        }
    }
}