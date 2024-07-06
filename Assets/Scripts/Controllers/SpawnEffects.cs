using DG.Tweening;
using UnityEngine;

namespace Game.EntitySystem
{
	[RequireComponent(typeof(AudioSource))]
	public class SpawnEffects : MonoBehaviour
	{
		[SerializeField] private GameObject _spawnVFX;
		[SerializeField] private float _animationDuration = 1f;

		private void Start()
		{
			transform.localScale = Vector3.zero;
			transform.DOScale(Vector3.one, _animationDuration).SetEase(Ease.OutBack);
			if (_spawnVFX != null)
			{
				Instantiate(_spawnVFX, transform.position, Quaternion.identity);
			}
			GetComponent<AudioSource>()?.Play();
		}
	}
}
