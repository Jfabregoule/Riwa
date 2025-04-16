using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "LightProbeData", menuName = "Lighting/Light Probe Data")]
public class LightProbeData : ScriptableObject
{
    public SphericalHarmonicsL2[] bakedProbes;
}
