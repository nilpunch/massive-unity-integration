// using System.Runtime.CompilerServices;
// using Unity.IL2CPP.CompilerServices;
//
// namespace Massive
// {
// 	public readonly struct UnmanagedView<T1, T2> where T1 : unmanaged where T2 : unmanaged
// 	{
// 		private readonly IReadOnlyDataSet<T1> _components1;
// 		private readonly IReadOnlyDataSet<T2> _components2;
//
// 		public UnmanagedView(IRegistry registry)
// 		{
// 			_components1 = registry.Components<T1>();
// 			_components2 = registry.Components<T2>();
// 		}
// 		
// 		public UnmanagedView(IReadOnlyDataSet<T1> components1, IReadOnlyDataSet<T2> components2)
// 		{
// 			_components1 = components1;
// 			_components2 = components2;
// 		}
// 		
// 		public Enumerator GetEnumerator()
// 		{
// 			return new Enumerator(_components1, _components2);
// 		}
//
// 		public unsafe struct Enumerator
// 		{
// 			private readonly T1* _firstData;
// 			private readonly T2* _secondData;
// 			private readonly IReadOnlySet _smallestSet;
// 			private readonly IReadOnlySet _otherSet;
// 			private readonly int* _smallestIds;
//
// 			private readonly delegate*<int, T1*, T2*, int, int, Entry> _dataSelector;
// 			
// 			private int _smallestDense;
// 			private int _entityId;
// 			private int _otherDense;
//
// 			public Enumerator(IReadOnlyDataSet<T1> first, IReadOnlyDataSet<T2> second)
// 			{
// 				fixed (T1* firstData = first.Data)
// 				{
// 					_firstData = firstData;
// 				}
// 				fixed (T2* secondData = second.Data)
// 				{
// 					_secondData = secondData;
// 				}
//
// 				if (first.Count <= second.Count)
// 				{
// 					_smallestSet = first;
// 					_otherSet = second;
// 					_dataSelector = &SelectFirstSecond;
// 				}
// 				else
// 				{
// 					_smallestSet = second;
// 					
// 					_otherSet = first;
// 					_dataSelector = &SelectSecondFirst;
// 				}
//
// 				fixed (int* smallestIds = _smallestSet.Ids)
// 				{
// 					_smallestIds = smallestIds;
// 				}
// 				
// 				_smallestDense = _smallestSet.Count;
// 				_entityId = 0;
// 				_otherDense = 0;
// 			}
//
// 			public bool MoveNext()
// 			{
// 				while (--_smallestDense >= 0)
// 				{
// 					_entityId = _smallestIds[_smallestDense];
// 					if (_otherSet.TryGetDense(_entityId, out _otherDense))
// 					{
// 						return true;
// 					}
// 				}
//
// 				return false;
// 			}
//
// 			public void Reset()
// 			{
// 				_smallestDense = _smallestSet.Count;
// 			}
//
// 			public unsafe Entry Current
// 			{
// 				[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 				get => _dataSelector(_entityId, _firstData, _secondData, _smallestDense, _otherDense);
// 			}
//
// 			private static Entry SelectFirstSecond(int entityId, T1* firstData, T2* secondData, int firstDense, int secondDense)
// 			{
// 				return new Entry(entityId, firstData + firstDense, secondData + secondDense);
// 			}
// 			
// 			private static Entry SelectSecondFirst(int entityId, T1* firstData, T2* secondData, int secondDense, int firstDense)
// 			{
// 				return new Entry(entityId, firstData + firstDense, secondData + secondDense);
// 			}
// 		}
// 		
// 		public readonly unsafe struct Entry
// 		{
// 			private readonly int _entityId;
// 			private readonly T1* _first;
// 			private readonly T2* _second;
//
// 			public Entry(int entityId, T1* first, T2* second)
// 			{
// 				_entityId = entityId;
// 				_first = first;
// 				_second = second;
// 			}
// 			
// 			[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 			public void Deconstruct(out int entityId, out T1* first, out T2* second)
// 			{
// 				entityId = _entityId;
// 				first = _first;
// 				second = _second;
// 			}
// 			
// 			[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 			public void Deconstruct(out T1* first, out T2* second)
// 			{
// 				first = _first;
// 				second = _second;
// 			}
// 		}
// 	}
// }
