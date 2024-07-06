using UnityEngine;

public class StaminCircle : MonoBehaviour
{
    public Material _Material;
    [Range(0f,1.0f)]
    public  float colorFill;
    [Range(0f,1.0f)]
    public float containFill;
    private bool isResume = false;

    // Update is called once per frame

    private void Start()
    {
        ResetFill();
    }

    public void ResetFill()
    {
        containFill = 1;
        isResume = false;
    }

    public void SetContainFill (float value)
    {
        containFill = value;
    }
    
    void Update()
    {
        _Material.SetFloat("_BarFill", containFill);
        colorFill = 1 - containFill;
        _Material.SetFloat("_FadeStart", colorFill);

        var faceTrans = Camera.main.transform.eulerAngles;
        // 始终朝向摄像机
        transform.eulerAngles = new Vector3(faceTrans.x, faceTrans.y, faceTrans.z);
    }

}
