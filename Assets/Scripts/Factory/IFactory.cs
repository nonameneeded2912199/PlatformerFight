using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerFight.Factory
{
	/// <summary>
	/// Represents a factory.
	/// </summary>
	/// <typeparam name="T">Specifies the type to create.</typeparam>
	public interface IFactory<T>
	{
		T Create();
	}
}