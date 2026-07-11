using UnityEngine;
using UnityEditor;

public class CopyAnimatorElementPath : EditorWindow
{
	[MenuItem("GameObject/Copy Path For Animations %l")]

	public static void FindAndCopyAnimatorPath()
	{
		foreach (var gameObject in Selection.gameObjects)
		{
			if (!gameObject.transform.parent) return;
			var animator = gameObject.transform.parent.GetComponentInParent<Animator>(true);
			var root = animator == null ? gameObject.transform.root : animator.transform;
			var path = AnimationUtility.CalculateTransformPath(gameObject.transform, root);
			Debug.Log("<color=yellow>Path copied to clipboard!</color>  " + path);
			GUIUtility.systemCopyBuffer = path;
		}
	}
}