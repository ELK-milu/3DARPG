using System;
using UnityUtils;

public class Preconditions {
	Preconditions() { }
    
	public static T CheckNotNull<T>(T reference) {
		return CheckNotNull(reference, null);
	}

	/// <summary>
	/// 检测Object是否为空，若为空抛出异常
	/// </summary>
	/// <param name="reference">检测物体</param>
	/// <param name="message">异常信息</param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	/// <exception cref="ArgumentNullException"></exception>
	public static T CheckNotNull<T>(T reference, string message) {
		// Can find OrNull Extension Method (and others) here: https://github.com/adammyhre/Unity-Utils
		if (reference is UnityEngine.Object obj && obj.OrNull() == null) {
			throw new ArgumentNullException(message);
		}
		if (reference is null) {
			throw new ArgumentNullException(message);
		}
		return reference;
	}

	public static void CheckState(bool expression) {
		CheckState(expression, null);
	}

	public static void CheckState(bool expression, string messageTemplate, params object[] messageArgs) {
		CheckState(expression, string.Format(messageTemplate, messageArgs));
	}

	public static void CheckState(bool expression, string message) {
		if (expression) {
			return;
		}

		throw message == null ? new InvalidOperationException() : new InvalidOperationException(message);
	}
}
