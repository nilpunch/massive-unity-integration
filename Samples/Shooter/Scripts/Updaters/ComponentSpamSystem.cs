using System;
using Massive.Netcode;
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
		public override void Init(Simulation simulation)
		{
			
			simulation.Registry.Set<TestState64<long, double, int>>();
			simulation.Registry.Set<TestState64<double, long, int>>();
			simulation.Registry.Set<TestState64<int, double, long>>();
			simulation.Registry.Set<TestState64<double, int, long>>();
			simulation.Registry.Set<TestState64<long, short, byte>>();
			simulation.Registry.Set<TestState64<short, ushort, int>>();
			simulation.Registry.Set<TestState64<ushort, ulong, float>>();
			simulation.Registry.Set<TestState64<ulong, double, decimal>>();
			simulation.Registry.Set<TestState64<decimal, char, byte>>();
			simulation.Registry.Set<TestState64<char, bool, int>>();
			simulation.Registry.Set<TestState64<bool, byte, sbyte>>();
			simulation.Registry.Set<TestState64<sbyte, float, ushort>>();
			simulation.Registry.Set<TestState64<int, double, bool>>();
			simulation.Registry.Set<TestState64<double, decimal, char>>();
			simulation.Registry.Set<TestState64<long, bool, float>>();
			simulation.Registry.Set<TestState64<short, int, ulong>>();
			simulation.Registry.Set<TestState64<ushort, sbyte, decimal>>();
			simulation.Registry.Set<TestState64<ulong, char, double>>();
			simulation.Registry.Set<TestState64<decimal, bool, short>>();
			simulation.Registry.Set<TestState64<char, int, byte>>();
			simulation.Registry.Set<TestState64<bool, ulong, sbyte>>();
			simulation.Registry.Set<TestState64<sbyte, short, ushort>>();
			simulation.Registry.Set<TestState64<float, ulong, int>>();
			simulation.Registry.Set<TestState64<double, sbyte, long>>();
			simulation.Registry.Set<TestState64<int, char, decimal>>();
			simulation.Registry.Set<TestState64<long, decimal, bool>>();
			simulation.Registry.Set<TestState64<short, double, byte>>();
			simulation.Registry.Set<TestState64<ushort, float, char>>();
			simulation.Registry.Set<TestState64<ulong, int, bool>>();
			simulation.Registry.Set<TestState64<decimal, short, ulong>>();
			simulation.Registry.Set<TestState64<char, ushort, float>>();
			simulation.Registry.Set<TestState64<bool, double, sbyte>>();
			simulation.Registry.Set<TestState64<sbyte, long, ushort>>();
			simulation.Registry.Set<TestState64<float, decimal, char>>();
			simulation.Registry.Set<TestState64<double, byte, int>>();
			simulation.Registry.Set<TestState64<int, sbyte, ulong>>();
			simulation.Registry.Set<TestState64<long, ushort, decimal>>();
			simulation.Registry.Set<TestState64<short, char, double>>();
			simulation.Registry.Set<TestState64<ushort, bool, float>>();
			simulation.Registry.Set<TestState64<ulong, byte, short>>();
			simulation.Registry.Set<TestState64<decimal, int, char>>();
			simulation.Registry.Set<TestState64<char, ulong, bool>>();
			simulation.Registry.Set<TestState64<bool, short, double>>();
			simulation.Registry.Set<TestState64<sbyte, decimal, ushort>>();
			simulation.Registry.Set<TestState64<float, bool, byte>>();
			simulation.Registry.Set<TestState64<double, ushort, sbyte>>();
			simulation.Registry.Set<TestState64<int, float, char>>();
			simulation.Registry.Set<TestState64<long, byte, ulong>>();
			simulation.Registry.Set<TestState64<short, decimal, bool>>();
			simulation.Registry.Set<TestState64<ushort, double, int>>();
		}

		public override void UpdateFrame(float deltaTime)
		{
		}
	}
}
