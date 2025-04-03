using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]

public class ChangeTime : MonoBehaviour
{
    private float radius;
    public float baseRadius = 0f;
    public float maxRadius = 50f;
    public bool isActivated;
    private float alpha = 0;
    public ParticleSystem Sphere;
    public ParticleSystem SphereElectricity;
    private ParticleSystem.ShapeModule shapeModule;
    private ParticleSystem.EmissionModule emissionModule;
    public ParticleSystem AmbianceParticles;
    private ParticleSystem.ShapeModule AmbianceShapeModule;
    private ParticleSystem.EmissionModule AmbianceEmissionModule;
    private bool particleActivated = false;
    public CinemachineImpulseSource impulseSource;
    public ParticleSystem Flash;
    private int past;
    private int present;

    public delegate void TimeEvent();
    public event TimeEvent OnTimeChangeEnd;

    private void Start()
    {

        shapeModule = SphereElectricity.shape;
        emissionModule = SphereElectricity.emission;

        AmbianceShapeModule = AmbianceParticles.shape;
        AmbianceEmissionModule = AmbianceParticles.emission;
        Shader.SetGlobalInt("_PresentEnum", 1);
        Shader.SetGlobalInt("_PastEnum", 0);
        past = Shader.GetGlobalInt("_PresentEnum");
        present = Shader.GetGlobalInt("_PastEnum");
    }

    // Update is called once per frame
    void Update()
    {
        
        Shader.SetGlobalVector("_Position", transform.position);
        Shader.SetGlobalFloat("_Radius",radius);

        if (isActivated)
        {
            if (!particleActivated)
            {
                Sphere.Play();
                SphereElectricity.Play();
                AmbianceParticles.Play();
                Flash.Play();
                impulseSource.GenerateImpulse();
                particleActivated = true;
            }
            alpha += Time.deltaTime / 4f;
            alpha = Mathf.Clamp01(alpha);
            radius = Mathf.Lerp(baseRadius, maxRadius, alpha);
            shapeModule.radius = radius;
            AmbianceShapeModule.radius = radius;
            emissionModule.rateOverTimeMultiplier = Mathf.Lerp(2000,10000,alpha);
            AmbianceEmissionModule.rateOverTimeMultiplier = Mathf.Lerp(20,40,alpha);
            if(alpha == 1)
            {
                isActivated = false;
                UpdateShaders();
                OnTimeChangeEnd?.Invoke();
            }
        }
    }

    public void UpdateShaders()
    {
        Shader.SetGlobalInt("_PresentEnum", 1 - present);
        Shader.SetGlobalInt("_PastEnum", 1 - past);
        past = 1 - past;
        present = 1 - present;
        radius = 0;
        Shader.SetGlobalFloat("_Radius", radius);
        particleActivated = false;
        alpha = 0;
    }

}
