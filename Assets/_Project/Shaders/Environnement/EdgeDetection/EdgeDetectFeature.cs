using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EdgeDetectFeature : ScriptableRendererFeature
{
    class EdgeDetectPass : ScriptableRenderPass
    {
        private Material _edgeMat;
        private RTHandle _renderTargetHandle;

        public EdgeDetectPass(Material material)
        {
            this._edgeMat = material;
        }

        public void Setup(RTHandle cameraColorTargetHandle)
        {
            this._renderTargetHandle = cameraColorTargetHandle;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (_edgeMat == null) return;

            CommandBuffer cmd = CommandBufferPool.Get("Edge Detection");

            // Create a temporary RTHandle for camera color
            var desc = renderingData.cameraData.cameraTargetDescriptor;
            desc.depthBufferBits = 0;
            RTHandle tempColorRT = RTHandles.Alloc(desc, name: "_CameraColorTexture");

            // Copy camera color to temp RT
            RenderingUtils.Blit(cmd, _renderTargetHandle, tempColorRT);
            // Set it as global texture for shader
            cmd.SetGlobalTexture("_CameraColorTexture", tempColorRT);

            // Final blit with outline effect
            RenderingUtils.Blit(cmd, tempColorRT, _renderTargetHandle, _edgeMat);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);

            RTHandles.Release(tempColorRT);
        }
    }

        [System.Serializable]
    public class EdgeDetectSettings
    {
        public Material edgeDetectMaterial;
    }

    public EdgeDetectSettings settings = new EdgeDetectSettings();
    private EdgeDetectPass edgePass;

    public override void Create()
    {
        edgePass = new EdgeDetectPass(settings.edgeDetectMaterial)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.edgeDetectMaterial != null)
        {
            edgePass.Setup(renderer.cameraColorTargetHandle);
            renderer.EnqueuePass(edgePass);
        }
    }
}
