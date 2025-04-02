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

    private void Start()
    {

        shapeModule = SphereElectricity.shape;
        emissionModule = SphereElectricity.emission;

        AmbianceShapeModule = AmbianceParticles.shape;
        AmbianceEmissionModule = AmbianceParticles.emission;
        Shader.SetGlobalInt("_Test", 1);
        Shader.SetGlobalInt("_Test2", 0);
        past = Shader.GetGlobalInt("_Test2");
        present = Shader.GetGlobalInt("_Test");
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
            }
        }
    }

    public void UpdateShaders()
    {
        Shader.SetGlobalInt("_Test", 1 - present);
        Shader.SetGlobalInt("_Test2", 1 - past);
        past = 1 - past;
        present = 1 - present;
        radius = 0;
        Shader.SetGlobalFloat("_Radius", radius);
        particleActivated = false;
        alpha = 0;
    }

}
