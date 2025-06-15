using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class CutoutMaskUi : Image
{
    private static readonly int StencilComp = Shader.PropertyToID("_StencilComp");
    private Material _runtimeMaterial;

    protected override void OnEnable()
    {
        base.OnEnable();
        RefreshMaterial();
    }

    public override Material materialForRendering
    {
        get
        {
            if (_runtimeMaterial == null)
                RefreshMaterial();

            return _runtimeMaterial;
        }
    }

    public void RefreshMaterial()
    {
        if (_runtimeMaterial != null)
            DestroyImmediate(_runtimeMaterial);

        _runtimeMaterial = new Material(base.materialForRendering);
        _runtimeMaterial.SetInt(StencilComp, (int)CompareFunction.NotEqual);
    }
} 
