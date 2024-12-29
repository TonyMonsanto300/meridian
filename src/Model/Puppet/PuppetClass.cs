using XenWorld.src.Loader.Puppet;

namespace XenWorld.src.Model.Puppet {
    public class PuppetClass {
        public PuppetClassEnum Name;
        public PuppetArchetype Archetype;

        public PuppetClass(PuppetClassEnum name) {
            this.Name = name;
        }
    }
}
