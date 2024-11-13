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
        // Set the RenderTexture to use ARGB32 format for compatibility with WebGL and enable alpha
        m_renderTexture = new RenderTexture(size, size, 0, RenderTextureFormat.ARGB32);
        
        // Enable random write for compatibility with D3D11
        m_renderTexture.enableRandomWrite = true;
        
        // Ensure it's created for use in WebGL
        m_renderTexture.Create();

        // Attach the RenderTexture to the material on the GameObject
        UnityEngine.Renderer renderer = GetComponent<UnityEngine.Renderer>();
        Material material = renderer.material;
        material.mainTexture = m_renderTexture;

        // Initialize the Rive RenderQueue and Renderer
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

        // WebGL-specific handling: Clear the RenderTexture manually for transparency
        Graphics.SetRenderTarget(m_renderTexture);
        GL.Clear(true, true, new UnityEngine.Color(0, 0, 0, 0)); // Clear with transparent color
        Graphics.SetRenderTarget(null);
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

    // Function to update the direction input in the state machine
    public void UpdateDirectionInput(int direction)
    {
        if (m_stateMachine == null) return;

        SMIInput directionInput = m_stateMachine.GetNumber("Direction"); 
        if (directionInput is SMINumber directionNumber)
        {
            directionNumber.Value = direction;
        }
    }

    // Function to update the level state based on GameManager's input
    public void UpdateLevelState(int level)
    {
        if (m_stateMachine == null) return;

        // Access the "Level" input in the state machine and set its value
        SMIInput levelInput = m_stateMachine.GetNumber("Level"); 
        if (levelInput is SMINumber levelNumber)
        {
            levelNumber.Value = level;
        }
    }
}
