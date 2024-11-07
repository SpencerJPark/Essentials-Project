using UnityEngine;
using UnityEngine.Rendering;
using Rive;

public class RiveTexture : MonoBehaviour
{
    public Asset asset;
    public Fit fit = Fit.contain;
    public int size = 512;

    private RenderTexture m_renderTexture;
    private Rive.RenderQueue m_renderQueue;
    private Rive.Renderer m_riveRenderer;
    private CommandBuffer m_commandBuffer;

    private File m_file;
    private Artboard m_artboard;
    private StateMachine m_stateMachine;
    public StateMachine stateMachine => m_stateMachine;

    private void Awake()
    {
        m_renderTexture = new RenderTexture(TextureHelper.Descriptor(size, size));
        m_renderTexture.Create();

        UnityEngine.Renderer renderer = GetComponent<UnityEngine.Renderer>();
        Material material = renderer.material;
        material.mainTexture = m_renderTexture;

        m_renderQueue = new Rive.RenderQueue(m_renderTexture);
        m_riveRenderer = m_renderQueue.Renderer();

        if (asset != null)
        {
            m_file = Rive.File.Load(asset);
            m_artboard = m_file.Artboard(0);
            m_stateMachine = m_artboard?.StateMachine();
        }

        if (m_artboard != null && m_renderTexture != null)
        {
            m_riveRenderer.Align(fit, Alignment.Center, m_artboard);
            m_riveRenderer.Draw(m_artboard);

            m_commandBuffer = new CommandBuffer();
            m_commandBuffer.SetRenderTarget(m_renderTexture);
            m_commandBuffer.ClearRenderTarget(true, true, UnityEngine.Color.clear, 0.0f);
            m_riveRenderer.AddToCommandBuffer(m_commandBuffer);
        }
    }

    private void Update()
    {
        m_riveRenderer.Submit();
        GL.InvalidateState();
        
        if (m_stateMachine != null)
        {
            m_stateMachine.Advance(Time.deltaTime);
        }
    }

    // New function to update the direction input in the state machine
    public void UpdateDirectionInput(int direction)
    {
        if (m_stateMachine == null) return;

        // Access the "Direction" input in the state machine and set its value
        SMIInput directionInput = m_stateMachine.GetNumber("Direction"); // Replace "Direction" with the actual name
        if (directionInput is SMINumber directionNumber)
        {
            directionNumber.Value = direction;
        }
    }
}
