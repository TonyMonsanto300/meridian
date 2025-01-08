using System.Collections.Generic;

namespace XenWorld.src.Model.Puppet {
    public enum ResourceSetEnum {
        Default
    }

    public static class ResourceSetDictionary {
        public static Dictionary<ResourceSetEnum, List<PuppetResource>> Context
            = new Dictionary<ResourceSetEnum, List<PuppetResource>>();

        public static void LoadResourceSet(ResourceSetEnum resourceSetEnum, List<PuppetResource> puppetResources) {
            Context.Add(resourceSetEnum, puppetResources);
        }
    }
}