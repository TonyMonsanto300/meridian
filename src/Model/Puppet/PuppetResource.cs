namespace XenWorld.src.Model.Puppet {
    public enum ResourceTypeEnum {
        Mana,
        Energy,
        Prayer,
        Miasma,
        Combo,
        Fury,
        Rune
    }
    public class PuppetResource {
        public ResourceTypeEnum Type { get; set; }
        public int Max { get; set; } = 0;
        public int Current { get; set; } = 0;

        public PuppetResource(ResourceTypeEnum type, int max, bool fill = true) {
            Type = type;
            Max = max;
            if (fill) {
                Current = max;
            } else {
                Current = 0;
            }
        }
    }
}
