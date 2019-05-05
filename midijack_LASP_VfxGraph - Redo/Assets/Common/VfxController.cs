using UnityEngine;
using UnityEngine.Experimental.VFX;

public class VfxController : MonoBehaviour
{
    public VisualEffect _target = null;
    public string _parameterName = "";

    public float parameterValue {
        set { _target.SetFloat(_parameterName, value); }
    }
}
