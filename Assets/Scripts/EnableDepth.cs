using UnityEngine;

[ExecuteInEditMode]
public class EnableDepth : MonoBehaviour
{
    private void OnEnable()
    {
        if (Camera.main != null)
            Camera.main.depthTextureMode = DepthTextureMode.Depth;
    }
}