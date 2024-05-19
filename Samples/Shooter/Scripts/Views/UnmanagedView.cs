// using System;
// using System.Runtime.CompilerServices;
// using Unity.IL2CPP.CompilerServices;
//
// namespace Massive
// {
// 	public readonly struct UnmanagedView<T> where T : unmanaged
// 	{
// 		private readonly IReadOnlyDataSet<T> _components;
//
// 		public UnmanagedView(IRegistry registry)
// 		{
// 			_components = registry.Components<T>();
// 		}
// 		
// 		public UnmanagedView(IReadOnlyDataSet<T> components)
// 		{
// 			_components = components;
// 		}
// 		
// 		public Enumerator GetEnumerator()
// 		{
// 			return new Enumerator(_components);
// 		}
//
// 		public unsafe struct Enumerator
// 		{
// 			private readonly IReadOnlySet _dataSet;
// 			private readonly T* _data;
// 			private readonly int* _ids;
//
// 			private int _index;
//
// 			private Entry _entry;
//
// 			public Enumerator(IReadOnlyDataSet<T> dataSet)
// 			{
// 				_dataSet = dataSet;
// 				
// 				fixed (T* data = dataSet.Data)
// 				{
// 					_data = data;
// 				}
// 				
// 				fixed (int* ids = dataSet.Ids)
// 				{
// 					_ids = ids;
// 				}
// 				
// 				_index = _dataSet.Count;
//
// 				_entry = default;
// 			}
//
// 			[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 			public bool MoveNext()
// 			{
// 				return --_index >= 0;
// 			}
//
// 			[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 			public void Reset()
// 			{
// 				_index = _dataSet.Count;
// 			}
//
// 			public unsafe Entry Current
// 			{
// 				[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 				get
// 				{
// 					// _entry.EntityId = _ids[_index];
// 					_entry.Data = _data + _index;
// 					return _entry;
// 				}
// 			}
// 		}
// 		
// 		public unsafe struct Entry
// 		{
// 			public int EntityId;
// 			public T* Data;
// 			
// 			[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 			public void Deconstruct(out int entityId, out T* data)
// 			{
// 				entityId = EntityId;
// 				data = Data;
// 			}
// 			
// 			[MethodImpl(MethodImplOptions.AggressiveInlining)]
// 			public void Deconstruct(out int entityId)
// 			{
// 				entityId = EntityId;
// 			}
// 		}
// 	}
// }
