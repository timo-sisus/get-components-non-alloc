using System.Collections.Generic;
using UnityEngine;

namespace Sisus
{
	/// <summary>
	/// Extensions methods for <see cref="GameObject"/> that can be used to easily get components of a particular type without allocating any garbage.
	/// </summary>
	public static class GetComponentExtensions
	{
		/// <summary>
		/// Gets references to all components of type <typeparamref name="TComponent"/> on the <paramref name="gameObject"/>,
		/// without allocating any garbage.
		/// <para>
		/// The result of this method should never be cached or reused; it should either be used with a using statement,
		/// so that it gets disposed when leaving the method scope, or iterated once with a foreach statement,
		/// in which case it gets disposed automatically at the end of the iteration.
		/// </para>
		/// </summary>
		/// <typeparam name="TComponent"> The type of elements in the list; either a <see cref="Component"/> or interface type. </typeparam>
		/// <param name="gameObject"> The game object to search. </param>
		/// <returns> A list containing all matching components of type <typeparamref name="TComponent"/>. </returns>
		/// <example>
		/// <code>
		/// foreach(var component in gameObject.GetComponentsNonAlloc{Component}())
		/// {
		/// 	// do something with component
		/// }
		/// </code>
		/// </example>
		/// <example>
		/// <code>
		/// using var components = gameObject.GetComponentsNonAlloc{Component}();
		/// // do something with components
		/// </code>
		/// </example>
		public static ComponentCollection<TComponent> GetComponentsNonAlloc<TComponent>(this GameObject gameObject) => ComponentCollection<TComponent>.GetFrom(gameObject);

		/// <summary>
		/// Gets references to all components of type <typeparamref name="TComponent"/> on the <paramref name="gameObject"/>,
		/// without allocating any garbage.
		/// <para>
		/// The result of this method should never be cached or reused; it should either be used with a using statement,
		/// so that it gets disposed when leaving the method scope, or iterated once with a foreach statement,
		/// in which case it gets disposed automatically at the end of the iteration.
		/// </para>
		/// </summary>
		/// <param name="gameObject"> The game object to search. </param>
		/// <param name="type"> The type of component to search for; either a <see cref="Component"/> or an interface type. </param>
		/// <returns> A list containing all matching components of type <typeparamref name="TComponent"/>. </returns>
		/// <example>
		/// <code>
		/// foreach(var component in gameObject.GetComponentsNonAlloc{Component}())
		/// {
		///		// do something with component
		/// }
		/// </code>
		/// </example>
		/// <example>
		/// <code>
		/// using var components = gameObject.GetComponentsNonAlloc{Component}();
		/// // do something with components
		/// </code>
		/// </example>
		public static ComponentCollection<Component> GetComponentsNonAlloc([DisallowNull] this GameObject gameObject, [DisallowNull] Type type) => ComponentCollection<Component>.GetFrom(gameObject, type);

		/// <summary>
		/// Gets a reference to the last component of type <typeparamref name="TComponent"/> on the <paramref name="gameObject"/>, if at least one is found; otherwise, <see langword="null"/>.
		/// <para>
		/// Does not allocate any garbage.
		/// </para>
		/// </summary>
		/// <typeparam name="TComponent"> The type of component to search for; either a <see cref="Component"/> or an interface type. </typeparam>
		/// <param name="gameObject"> The game object to search. </param>
		/// <returns>
		/// A component of type <typeparamref name="TComponent"/> or <see langword="null"/>.
		/// </returns>
		[return: MaybeNull]
		public static TComponent GetLastComponent<TComponent>([DisallowNull] this GameObject gameObject)
		{
			using var components = ComponentCollection<TComponent>.GetFrom(gameObject);
			int count = components.Count;
			return count > 0 ? components[count - 1] : default;
		}

		/// <summary>
		/// Gets a reference to the last component of type <typeparamref name="TComponent"/> on the <paramref name="gameObject"/>, if at least one is found; otherwise, <see langword="null"/>.
		/// <para>
		/// Does not allocate any garbage.
		/// </para>
		/// </summary>
		/// <param name="gameObject"> The game object to search. </param>
		/// <param name="type"> The type of component to search for; either a <see cref="Component"/> or an interface type. </param>
		/// <returns>
		/// A component of type <paramref name="type"/> or <see langword="null"/>.
		/// </returns>
		public static Component GetLastComponent(this GameObject gameObject, Type type)
		{
			using var components = ComponentCollection<Component>.GetFrom(gameObject, type);
			int count = components.Count;
			return count > 0 ? components[count - 1] : null;
		}
	}
}
