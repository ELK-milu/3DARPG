using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Architecture.StateSystem
{
	/// <summary>
	/// 管理单个角色的状态系统
	/// </summary>
	public class StateSystem : MonoBehaviour
	{
		[SerializeField]
		private StateData _stateDatas;
		private StateController _stateController;
		private void Awake()
		{

		}

	}

}
