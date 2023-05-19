using PequennosEjerciciosApi.Enums;

namespace PequennosEjerciciosApi.Utils
{
    public class RequestBattleArmy
    {
        public List<KeyValuePair<KindArmy, int>> kindArmyn { get; set; }
        public List<KeyValuePair<EvilArmy, int>> evilArmyn { get; set; }
    }
}
