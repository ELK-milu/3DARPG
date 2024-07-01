using UnityEngine;

namespace Architecture.AbilitySystem
{
	/// <summary>
	/// AbilitySystem存在的意义让AbilityController利用MonoBehaviour的一些特性进行交互
	/// </summary>
	[DefaultExecutionOrder(-1901)]
	[RequireComponent(typeof(AbilityView))]
	public class AbilitySystem : MonoBehaviour
	{
		[SerializeField]
		private AbilityData[] _abilityDatas;
		private AbilityController _abilityController;

		private void Awake()
		{
			var _view = GetComponent<AbilityView>();
			_abilityController = new AbilityController(_abilityDatas, _view);
		}

		private void OnEnable()
		{
			_abilityController.OnEnable();
		}

		private void OnDisable()
		{
			_abilityController.OnDisable();
		}

		private void Update()
		{
			_abilityController.Update(Time.deltaTime);
		}
	}
}
