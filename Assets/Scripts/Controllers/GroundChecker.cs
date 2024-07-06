using UnityEngine;

namespace Game.EntitySystem
{
	public class GroundChecker: MonoBehaviour
	{
		[SerializeField] float groundDistance = 0.08f;
		[SerializeField] float additionDistance = 0.08f;
		[SerializeField] LayerMask groundLayers;
		[SerializeField] float offsetToFeet = 0.7f; // 从Collider中心到底部的距离
		[SerializeField] Vector3 detectionOffset; // 向前检测的小球的偏移
		[SerializeField] float detectionForward; // 向前检测的偏移
		[SerializeField] float forwardDistance; // 向前检测的偏移

		public bool IsGrounded { get; private set; }


		void FixedUpdate()
		{
			Vector3 feetPosition = transform.position - Vector3.up * offsetToFeet;

			Vector3 transPos = feetPosition;
			// 原有的向下检测
			bool isGroundedBelow = Physics.SphereCast(transPos, groundDistance, Vector3.down, out _, groundDistance + additionDistance, groundLayers);

			// 新增的向前检测
			Vector3 forwardFeetPosition = feetPosition + transform.forward * detectionForward + detectionOffset;
			bool isGroundedForward = Physics.SphereCast(forwardFeetPosition, forwardDistance, transform.forward, out var hit, forwardDistance + additionDistance, groundLayers);
			// 结合两个检测结果
			IsGrounded = isGroundedBelow || isGroundedForward;
		}
		
		private void OnDrawGizmos()
		
		{
			Gizmos.color = Color.green; // 设置Gizmo的颜色

			// 绘制脚底下的检测范围
			Vector3 feetPosition = transform.position - Vector3.up * offsetToFeet;
			Gizmos.DrawWireSphere(feetPosition, groundDistance);

			// 绘制前方的检测范围
			Vector3 forwardFeetPosition = feetPosition + transform.forward * detectionForward + detectionOffset;
			Gizmos.DrawWireSphere(forwardFeetPosition, forwardDistance);
		}
	}
}
