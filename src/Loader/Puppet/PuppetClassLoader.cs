using System.Collections.Generic;
using XenWorld.src.Model.Puppet;
using XenWorld.src.Repository.Puppet;

namespace XenWorld.src.Loader.Puppet {
    public enum PuppetClassEnum {
        None
    }
    public class PuppetClassLoader {
        public PuppetClassLoader() { }

        private static List<PuppetClass> puppetClasses = new List<PuppetClass>() {
            new PuppetClass(PuppetClassEnum.None)
        };

        public static void SeedClasses() {
            puppetClasses.ForEach(puppetClass => {
                PuppetClassDictionary.LoadPuppetClass(puppetClass.Name, puppetClass);
            });
        }
    }
}
