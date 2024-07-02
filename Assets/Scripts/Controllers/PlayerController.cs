using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        float MoveX = Input.GetAxisRaw("Horizontal");
        float MoveZ = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(MoveX, 0, MoveZ).normalized;

        //1、AddForce
        //rb.AddForce(dir * speed * Time.deltaTime);
        //2、修改velocity
        //rb.velocity += dir * speed * Time.deltaTime;
        //3、MovePosition
        rb.MovePosition(transform.position + moveSpeed * Time.deltaTime * dir);
    }
}
