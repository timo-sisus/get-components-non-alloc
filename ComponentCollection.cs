using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sisus
{
	/// <summary>
	/// A list of components acquired using <see cref="GetComponentExtensions.GetComponentsNonAlloc"/>.
	/// <para>
	/// This object should never be cached or reused; it should either be used with a using statement,
	/// so that it gets disposed when leaving the method scope, or iterated once with a foreach statement,
	/// in which case it gets disposed automatically at the end of the iteration.
	/// </para>
	/// </summary>
	/// <typeparam name="TComponent"> The type of elements in the list; either a <see cref="Component"/> or interface type. </typeparam>
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
	public sealed class ComponentCollection<TComponent> : List<TComponent>, IDisposable
	{
		internal ComponentCollection(int capacity) : base(capacity) { }

		public void Dispose()
		{
			var cachedResults = GetComponentExtensions.Cached<TComponent>.collections;
			if(!cachedResults.Contains(this))
			{
				Clear();
				cachedResults.Add(this);
			}
		}

		public new Enumerator GetEnumerator() => new(this);

		public new struct Enumerator : IDisposable
		{
			private readonly ComponentCollection<TComponent> components;
			private int currentIndex;

			public Enumerator(ComponentCollection<TComponent> components)
			{
				#if DEBUG || SAFE_MODE
				if(components is null)
				{
					Debug.LogError($"Attempted to iterate over a null {nameof(ComponentCollection<object>)}<{typeof(TComponent).Name}>.");
					components = new(0);
				}
				#endif

				this.components = components;
				currentIndex = -1;
			}

			public bool MoveNext() => ++currentIndex < components.Count;

			public TComponent Current
			{
				get
				{
					#if DEBUG || SAFE_MODE
					if(currentIndex < 0 || currentIndex >= components.Count)
					{
						Debug.LogError($"Attempted to access {nameof(Current)} property of {nameof(ComponentCollection<object>)}<{typeof(TComponent).Name}>.{nameof(Enumerator)} after already reaching end of the list.", components.Count > 0 ? components[0] as Object : null);
						return default;
					}
					#endif

					return components[currentIndex];
				}
			}

			public void Dispose() => components.Dispose();
		}
	}
}
