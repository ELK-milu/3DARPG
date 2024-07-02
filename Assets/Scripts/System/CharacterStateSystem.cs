using UnityEngine;

namespace Architecture.StateSystem
{
	/// <summary>
	/// 管理单个角色的状态系统
	/// </summary>
	public class CharacterStateSystem : MonoBehaviour
	{
		[SerializeField]
		private CharacterStateData[] _characterStateData;
		private CharacterStateController _characterStateController;
		private void Awake()
		{
			var _view = GetComponent<CharacterStateView>();
			_characterStateController = new CharacterStateController(_characterStateData, _view);
		}

		private void OnEnable()
		{
			_characterStateController.OnEnable();
		}

		private void OnDisable()
		{
			_characterStateController.OnDisable();
		}
	}

}
