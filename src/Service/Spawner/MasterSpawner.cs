using System;
using System.Collections.Generic;

namespace XenWorld.src.Service.Spawner {
    public class MasterSpawner {
        private readonly List<AbstractSpawner> _spawners;

        public MasterSpawner(List<AbstractSpawner> spawners) {
            _spawners = spawners ?? throw new ArgumentNullException(nameof(spawners));
        }

        public void Execute() {
            foreach (var spawner in _spawners) {
                spawner.Spawn();
            }
        }
    }
}