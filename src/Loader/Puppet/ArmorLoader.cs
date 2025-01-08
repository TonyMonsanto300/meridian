using XenWorld.src.Model.Puppet.Equipment;
using XenWorld.src.Repository.Puppet;

namespace XenWorld.src.Loader.Puppet {
    public static class ArmorLoader {
        public static void SeedArmors() {
            ArmorDictionary.LoadArmor(ArmorEnum.DEFAULT, new PuppetArmor("Hide Tunic", 1, PuppetArmorClass.Light));
            ArmorDictionary.LoadArmor(ArmorEnum.PLAYER, new PuppetArmor("Bronze Plate", 3, PuppetArmorClass.Heavy));
        }
    }
}