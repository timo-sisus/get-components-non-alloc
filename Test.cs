#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace Sisus
{
	public static class Test
	{
		[MenuItem("Test/Test GetComponentsNonAlloc")]
		public static void TestGetComponentsNonAlloc()
		{
			if(Selection.activeGameObject is not GameObject gameObject || gameObject == null)
			{
				Debug.Log("No game object selected in the scene hierarchy.");
				return;
			}

			// Memory gets allocated on first call, but not after that
			using(gameObject.GetComponentsNonAlloc<Component>()) { }

			int counter = 0;

			long usedMemoryBefore = Profiler.GetMonoUsedSizeLong();
			foreach(var component in gameObject.GetComponentsNonAlloc<Component>())
			{
				counter++;
			}
			long usedMemoryAfter = Profiler.GetMonoUsedSizeLong();

			if(usedMemoryAfter > usedMemoryBefore)
			{
				Debug.LogError($"Allocated memory increased from {usedMemoryBefore} to {usedMemoryAfter}.", gameObject);
			}
			else
			{
				Debug.Log($"Acquired references to {counter} components on {gameObject.name} without generating any garbage.", gameObject);
			}
		}
	}
}
#endif
