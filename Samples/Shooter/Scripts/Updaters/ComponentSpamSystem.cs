using UnityEngine;

namespace Massive.Unity.Samples.Shooter
{
	public struct TestState64<T1, T2, T3>
	{
		public Quaternion Bytes16_1;
		public Quaternion Bytes16_2;
		public Quaternion Bytes16_3;
		public Quaternion Bytes16_4;
	}

	public class ComponentSpamSystem : UpdateSystem
	{
		public override void Init(Registry registry)
		{
			registry.Set<TestState64<long, double, int>>();
			registry.Set<TestState64<double, long, int>>();
			registry.Set<TestState64<int, double, long>>();
			registry.Set<TestState64<double, int, long>>();
			registry.Set<TestState64<long, short, byte>>();
			registry.Set<TestState64<short, ushort, int>>();
			registry.Set<TestState64<ushort, ulong, float>>();
			registry.Set<TestState64<ulong, double, decimal>>();
			registry.Set<TestState64<decimal, char, byte>>();
			registry.Set<TestState64<char, bool, int>>();
			registry.Set<TestState64<bool, byte, sbyte>>();
			registry.Set<TestState64<sbyte, float, ushort>>();
			registry.Set<TestState64<int, double, bool>>();
			registry.Set<TestState64<double, decimal, char>>();
			registry.Set<TestState64<long, bool, float>>();
			registry.Set<TestState64<short, int, ulong>>();
			registry.Set<TestState64<ushort, sbyte, decimal>>();
			registry.Set<TestState64<ulong, char, double>>();
			registry.Set<TestState64<decimal, bool, short>>();
			registry.Set<TestState64<char, int, byte>>();
			registry.Set<TestState64<bool, ulong, sbyte>>();
			registry.Set<TestState64<sbyte, short, ushort>>();
			registry.Set<TestState64<float, ulong, int>>();
			registry.Set<TestState64<double, sbyte, long>>();
			registry.Set<TestState64<int, char, decimal>>();
			registry.Set<TestState64<long, decimal, bool>>();
			registry.Set<TestState64<short, double, byte>>();
			registry.Set<TestState64<ushort, float, char>>();
			registry.Set<TestState64<ulong, int, bool>>();
			registry.Set<TestState64<decimal, short, ulong>>();
			registry.Set<TestState64<char, ushort, float>>();
			registry.Set<TestState64<bool, double, sbyte>>();
			registry.Set<TestState64<sbyte, long, ushort>>();
			registry.Set<TestState64<float, decimal, char>>();
			registry.Set<TestState64<double, byte, int>>();
			registry.Set<TestState64<int, sbyte, ulong>>();
			registry.Set<TestState64<long, ushort, decimal>>();
			registry.Set<TestState64<short, char, double>>();
			registry.Set<TestState64<ushort, bool, float>>();
			registry.Set<TestState64<ulong, byte, short>>();
			registry.Set<TestState64<decimal, int, char>>();
			registry.Set<TestState64<char, ulong, bool>>();
			registry.Set<TestState64<bool, short, double>>();
			registry.Set<TestState64<sbyte, decimal, ushort>>();
			registry.Set<TestState64<float, bool, byte>>();
			registry.Set<TestState64<double, ushort, sbyte>>();
			registry.Set<TestState64<int, float, char>>();
			registry.Set<TestState64<long, byte, ulong>>();
			registry.Set<TestState64<short, decimal, bool>>();
			registry.Set<TestState64<ushort, double, int>>();
		}

		public override void UpdateFrame(float deltaTime)
		{
		}
	}
}
