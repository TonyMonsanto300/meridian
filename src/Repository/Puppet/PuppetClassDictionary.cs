using System.Collections.Generic;
using XenWorld.src.Loader.Puppet;
using XenWorld.src.Model.Puppet;

namespace XenWorld.src.Repository.Puppet {
    public class PuppetClassDictionary {
        public static Dictionary<PuppetClassEnum, PuppetClass> Context = new Dictionary<PuppetClassEnum, PuppetClass>();

        public static void LoadPuppetClass(PuppetClassEnum name, PuppetClass puppetClass) {
            Context.Add(name, puppetClass);
        }
    }
}
