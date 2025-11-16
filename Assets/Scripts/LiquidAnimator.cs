using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Renderer))]
public class LiquidAnimator : MonoBehaviour
{
    private static readonly int FillAmountRemap = Shader.PropertyToID("_FillAmountRemap");
    private static readonly int FillAmount = Shader.PropertyToID("_FillAmount");
    private static readonly int Wobble = Shader.PropertyToID("_Wobble");
    private static readonly int WaveHeight = Shader.PropertyToID("_WaveHeight");
    private static readonly int WaveScale = Shader.PropertyToID("_WaveScale");
    private static readonly int WaveSpeed = Shader.PropertyToID("_WaveSpeed");
    public float fillAmount = 0.5f;
    Renderer rend;
    MaterialPropertyBlock block;
    Vector3 lastPos, velocity, smoothVelocity;
    Vector3 wobble, wobbleTarget;
    public float wobbleDecay = 2f;
    public float wobbleIntensity = 0.25f;
    public float waveBaseHeight = 0.02f;

    void OnEnable()
    {
        rend = GetComponent<Renderer>();
        block = new MaterialPropertyBlock();
        lastPos = transform.position;
        
        Bounds b = rend.bounds;
        float minY = rend.bounds.min.y - transform.position.y;
        float maxY = rend.bounds.max.y - transform.position.y;
        rend.GetPropertyBlock(block);
        block.SetVector(FillAmountRemap, new Vector2(minY, maxY));
        rend.SetPropertyBlock(block);
        
    }

    void Update()
    {
        rend.GetPropertyBlock(block);
        
        block.SetFloat(FillAmount, fillAmount);

        Vector3 pos = transform.position;
        velocity = (pos - lastPos) / Mathf.Max(Time.deltaTime, 1e-5f);
        lastPos = pos;

        smoothVelocity = Vector3.Lerp(smoothVelocity, velocity, Time.deltaTime * 5f);
        wobbleTarget = new Vector3(smoothVelocity.x, -smoothVelocity.z, 0) * wobbleIntensity;
        wobble = Vector3.Lerp(wobble, wobbleTarget, Time.deltaTime * wobbleDecay);
        block.SetVector(Wobble, new Vector2(wobble.x, wobble.y));

        float waveHeight = waveBaseHeight + wobble.magnitude * 0.05f;
        block.SetFloat(WaveHeight, waveHeight);
        block.SetFloat(WaveScale, 1.0f);
        block.SetFloat(WaveSpeed, 1.0f);

        rend.SetPropertyBlock(block);
    }
    
    void OnDrawGizmosSelected()
    {
        var r = GetComponent<MeshRenderer>();
        if (r == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(r.bounds.center, r.bounds.size);
    }

}