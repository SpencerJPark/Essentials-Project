using UnityEngine;
using UnityEngine.Rendering;
using Rive;

public class RiveWinScreen : MonoBehaviour
{
    public Asset asset;
    public Fit fit = Fit.contain;
    public int size = 512;
    public RenderTexture outputTexture;

    private Rive.RenderQueue m_renderQueue;
    private Rive.Renderer m_riveRenderer;
    private CommandBuffer m_commandBuffer;

    private File m_file;
    private Artboard m_artboard;
    private StateMachine m_stateMachine;
    private bool isPlaying = false; // Control when the animation should play

    private void Awake()
    {
        if (outputTexture == null)
        {
            outputTexture = new RenderTexture(size, size, 0);
            outputTexture.Create();
        }

        m_renderQueue = new Rive.RenderQueue(outputTexture);
        m_riveRenderer = m_renderQueue.Renderer();

        if (asset != null)
        {
            m_file = Rive.File.Load(asset);
            m_artboard = m_file.Artboard(0);
            m_stateMachine = m_artboard?.StateMachine(0);
        }

        if (m_artboard != null && outputTexture != null)
        {
            m_riveRenderer.Align(fit, Alignment.Center, m_artboard);
            m_riveRenderer.Draw(m_artboard);

            m_commandBuffer = new CommandBuffer();
            m_commandBuffer.SetRenderTarget(outputTexture);
            m_commandBuffer.ClearRenderTarget(true, true, UnityEngine.Color.clear, 0.0f); // Specify UnityEngine.Color to avoid ambiguity
            m_riveRenderer.AddToCommandBuffer(m_commandBuffer);
        }
    }

    private void Update()
    {
        if (isPlaying && m_stateMachine != null)
        {
            m_stateMachine.Advance(Time.deltaTime);
            m_riveRenderer.Submit();
            GL.InvalidateState();
        }
    }

    // Method to start the animation when the win screen is shown
    public void StartAnimation()
    {
        isPlaying = true;
    }

    private void OnDisable()
    {
        if (m_commandBuffer != null)
        {
            Camera.main?.RemoveCommandBuffer(CameraEvent.AfterEverything, m_commandBuffer);
        }
    }
}
