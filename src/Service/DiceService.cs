using XenWorld.Model;

namespace XenWorld.src.Service {
    public enum Dice {
        D4,
        D6,
        D8,
        D10,
        D12,
        D20
    }
    public class DiceService {

        public static int GetDieValue(Dice die) {
            switch (die) {
                case Dice.D4:
                    return 4;
                case Dice.D6:
                    return 6;
                case Dice.D8:
                    return 8;
                case Dice.D10:
                    return 10;
                case Dice.D12:
                    return 12;
                case Dice.D20:
                    return 20;
                default:
                    return 0;
            }
        }
    }
}
