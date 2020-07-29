using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
	// Utility functions for dealing with Vector3s
	public static Vector3 ElemwiseProduct(Vector3 first, Vector3 second)
	{
		return new Vector3(first.x * second.x, first.y * second.y, first.z * second.z);
	}

	public static Vector3 Absolute(Vector3 input)
	{
		return new Vector3(Mathf.Abs(input.x), Mathf.Abs(input.y), Mathf.Abs(input.z));
	}

	// Cartesian product of a Vector2 and an int to give a Vector3
	public static List<Vector2> Cartesian(List<float> inp1, List<float> inp2)
	{
		List<Vector2> output = new List<Vector2>();
		for (int i = 0; i < inp1.Count; i++)
		{
			for (int j = 0; j < inp2.Count; j++)
			{
				output.Add(new Vector2(inp1[i], inp2[j]));
			}
		}
		return output;
	}

	public static List<Vector2> Cartesian(List<int> inp1, List<float> inp2)
	{
		List<Vector2> output = new List<Vector2>();
		for (int i = 0; i < inp1.Count; i++)
		{
			for (int j = 0; j < inp2.Count; j++)
			{
				output.Add(new Vector2(inp1[i], inp2[j]));
			}
		}
		return output;
	}

	public static List<Vector2> Cartesian(List<float> inp1, List<int> inp2)
	{
		List<Vector2> output = new List<Vector2>();
		for (int i = 0; i < inp1.Count; i++)
		{
			for (int j = 0; j < inp2.Count; j++)
			{
				output.Add(new Vector2(inp1[i], inp2[j]));
			}
		}
		return output;
	}

	public static List<Vector2> Cartesian(List<int> inp1, List<int> inp2)
	{
		List<Vector2> output = new List<Vector2>();
		for (int i = 0; i < inp1.Count; i++)
		{
			for (int j = 0; j < inp2.Count; j++)
			{
				output.Add(new Vector2(inp1[i], inp2[j]));
			}
		}
		return output;
	}

	// The same but with a Vector3
	public static List<Vector3> Cartesian(List<Vector2> inp1, List<float> inp2)
	{
		List<Vector3> output = new List<Vector3>();
		for (int i = 0; i < inp1.Count; i++)
		{
			for (int j = 0; j < inp2.Count; j++)
			{
				output.Add(new Vector3(inp1[i].x, inp1[i].y, inp2[j]));
			}
		}
		return output;
	}

	public static List<Vector3> Cartesian(List<Vector2> inp1, List<int> inp2)
	{
		List<Vector3> output = new List<Vector3>();
		for (int i = 0; i < inp1.Count; i++)
		{
			for (int j = 0; j < inp2.Count; j++)
			{
				output.Add(new Vector3(inp1[i].x, inp1[i].y, inp2[j]));
			}
		}
		return output;
	}

	// The same but with Vector4
	public static List<Vector4> Cartesian(List<Vector3> inp1, List<float> inp2)
	{
		List<Vector4> output = new List<Vector4>();
		for (int i = 0; i < inp1.Count; i++)
		{
			for (int j = 0; j < inp2.Count; j++)
			{
				output.Add(new Vector4(inp1[i].x, inp1[i].y, inp1[i].z, inp2[j]));
			}
		}
		return output;
	}

	public static List<Vector4> Cartesian(List<Vector3> inp1, List<int> inp2)
	{
		List<Vector4> output = new List<Vector4>();
		for (int i = 0; i < inp1.Count; i++)
		{
			for (int j = 0; j < inp2.Count; j++)
			{
				output.Add(new Vector4(inp1[i].x, inp1[i].y, inp1[i].z, inp2[j]));
			}
		}
		return output;
	}

	public static List<Vector4> Cartesian(List<Vector2> inp1, List<int> inp2, List<float> inp3)
	{
		List<Vector4> output = new List<Vector4>();
		for (int i = 0; i < inp1.Count; i++)
		{
			for (int j = 0; j < inp2.Count; j++)
			{
				for (int k = 0; k < inp3.Count; k++)
				{
					output.Add(new Vector4(inp1[i].x, inp1[i].y, inp2[j], inp3[k]));
				}
			}
		}
		return output;
	}

	// In-place swap elements in a list
	public static void Swap<T>(List<T> list, int indexA, int indexB)
	{
		T tmp = list[indexA];
		list[indexA] = list[indexB];
		list[indexB] = tmp;
	}

	// Fisher-Yates (Durstenfeld-Knuth) in-place shuffle
	public static void Shuffle<T>(List<T> inp)
	{
		int elems = inp.Count;
		for (int i = elems - 1; i > 0; i--)
		{
			int j = (int)Mathf.Floor(Random.value * (i + 1));
			Swap<T>(inp, i, j);
		}
	}

	public static T Pop<T>(List<T> inp)
	{
		int i = inp.Count - 1;
		T r = inp[i];
		inp.RemoveAt(i);
		return r;
	}

	public static float RoundToMultiple(float first, float second)
	{
		return Mathf.Floor(first / second) * second;
	}
}
