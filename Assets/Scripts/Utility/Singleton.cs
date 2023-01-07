using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
	public static T Instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<T>();
			}
			return _instance;
		}
		set {
			_instance = value;
		}

	}
	private static T _instance;

	protected virtual void Awake() {
		if (_instance != null && _instance != this) {
			Destroy(gameObject);
			return;
		}

		_instance = GetComponent<T>();
	}
}