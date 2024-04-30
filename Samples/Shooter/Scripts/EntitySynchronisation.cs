﻿using System;
using System.Collections.Generic;

namespace Massive.Samples.Shooter
{
	public class EntitySynchronisation<TState>
	{
		private readonly AvailableEntities<TState> _entities;

		public EntitySynchronisation(EntityFactory<TState> entityFactory)
		{
			_entities = new AvailableEntities<TState>(entityFactory);
		}

		public void Synchronize(IReadOnlyDataSet<TState> data)
		{
			var aliveCount = data.Count;

			if (_entities.EntitiesCount < aliveCount)
			{
				_entities.Reserve(aliveCount - _entities.EntitiesCount);
			}
			else if (_entities.EntitiesCount > aliveCount)
			{
				_entities.Free(_entities.EntitiesCount - aliveCount);
			}

			IReadOnlyList<EntityRoot<TState>> entities = _entities.Entities;
			Span<TState> states = data.Data;
			for (int i = 0; i < aliveCount; i++)
			{
				entities[i].SyncState(ref states[i]);
			}
		}
	}
}
