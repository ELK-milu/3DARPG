using UnityEngine;

public class ForceTracer : MonoBehaviour
{
    // 通过在Inspector中调整此值，可以调整物体跟随鼠标的灵敏度
    public float sensitivity = 1.0f;

    private Vector3 offset; // 用于存储物体相对于鼠标位置的偏移量

    void Start()
    {
        // 初始化偏移量，确保物体开始时位于鼠标位置
        offset = transform.localPosition;
    }

    void Update()
    {
        // 获取鼠标在屏幕中的当前位置
        Vector3 mousePosition = Input.mousePosition;

        // 检查并处理边界情况：确保鼠标位置在屏幕范围内
        mousePosition.x = Mathf.Clamp(mousePosition.x, 0f, Screen.width);
        mousePosition.y = Mathf.Clamp(mousePosition.y, 0f, Screen.height);

        // 计算物体的新位置：鼠标位置加上之前的偏移量
        // 这样可以保持物体的相对位置不变，同时防止其移出屏幕
        Vector3 newPosition = mousePosition + offset;

        // 更新物体的本地位置
        transform.position = newPosition;
    }

}
