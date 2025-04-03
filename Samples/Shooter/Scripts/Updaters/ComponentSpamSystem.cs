using System;
using Mathematics.Fixed;
using UnityEngine;

namespace Massive.Unity.Samples.Shooter
{
	[Serializable]
	public struct TestState64<T1, T2, T3>
	{
		public Quaternion Bytes16_1;
		public Quaternion Bytes16_2;
		public Quaternion Bytes16_3;
		public Quaternion Bytes16_4;
	}

	public class ComponentSpamSystem : UpdateSystem
	{
		public override void Init(ServiceLocator serviceLocator)
		{
			var registry = serviceLocator.Find<World>();
			registry.SparseSet<TestState64<long, double, int>>();
			registry.SparseSet<TestState64<double, long, int>>();
			registry.SparseSet<TestState64<int, double, long>>();
			registry.SparseSet<TestState64<double, int, long>>();
			registry.SparseSet<TestState64<long, short, byte>>();
			registry.SparseSet<TestState64<short, ushort, int>>();
			registry.SparseSet<TestState64<ushort, ulong, float>>();
			registry.SparseSet<TestState64<ulong, double, decimal>>();
			registry.SparseSet<TestState64<decimal, char, byte>>();
			registry.SparseSet<TestState64<char, bool, int>>();
			registry.SparseSet<TestState64<bool, byte, sbyte>>();
			registry.SparseSet<TestState64<sbyte, float, ushort>>();
			registry.SparseSet<TestState64<int, double, bool>>();
			registry.SparseSet<TestState64<double, decimal, char>>();
			registry.SparseSet<TestState64<long, bool, float>>();
			registry.SparseSet<TestState64<short, int, ulong>>();
			registry.SparseSet<TestState64<ushort, sbyte, decimal>>();
			registry.SparseSet<TestState64<ulong, char, double>>();
			registry.SparseSet<TestState64<decimal, bool, short>>();
			registry.SparseSet<TestState64<char, int, byte>>();
			registry.SparseSet<TestState64<bool, ulong, sbyte>>();
			registry.SparseSet<TestState64<sbyte, short, ushort>>();
			registry.SparseSet<TestState64<float, ulong, int>>();
			registry.SparseSet<TestState64<double, sbyte, long>>();
			registry.SparseSet<TestState64<int, char, decimal>>();
			registry.SparseSet<TestState64<long, decimal, bool>>();
			registry.SparseSet<TestState64<short, double, byte>>();
			registry.SparseSet<TestState64<ushort, float, char>>();
			registry.SparseSet<TestState64<ulong, int, bool>>();
			registry.SparseSet<TestState64<decimal, short, ulong>>();
			registry.SparseSet<TestState64<char, ushort, float>>();
			registry.SparseSet<TestState64<bool, double, sbyte>>();
			registry.SparseSet<TestState64<sbyte, long, ushort>>();
			registry.SparseSet<TestState64<float, decimal, char>>();
			registry.SparseSet<TestState64<double, byte, int>>();
			registry.SparseSet<TestState64<int, sbyte, ulong>>();
			registry.SparseSet<TestState64<long, ushort, decimal>>();
			registry.SparseSet<TestState64<short, char, double>>();
			registry.SparseSet<TestState64<ushort, bool, float>>();
			registry.SparseSet<TestState64<ulong, byte, short>>();
			registry.SparseSet<TestState64<decimal, int, char>>();
			registry.SparseSet<TestState64<char, ulong, bool>>();
			registry.SparseSet<TestState64<bool, short, double>>();
			registry.SparseSet<TestState64<sbyte, decimal, ushort>>();
			registry.SparseSet<TestState64<float, bool, byte>>();
			registry.SparseSet<TestState64<double, ushort, sbyte>>();
			registry.SparseSet<TestState64<int, float, char>>();
			registry.SparseSet<TestState64<long, byte, ulong>>();
			registry.SparseSet<TestState64<short, decimal, bool>>();
			registry.SparseSet<TestState64<ushort, double, int>>();
		}

		public override void UpdateFrame(FP deltaTime)
		{
		}
	}
}
