using PequennosEjerciciosApi.Controllers.Exceptions;
using PequennosEjerciciosApi.Utils;

namespace PequennosEjerciciosApi.Controllers.Validations
{
    public class ValidationsBattlePokemon
    {
        public void ElementIsNull(RequestBattle requestBattle)
        {
            if (requestBattle == null) throw new ElementNullException("No puede ser vacio");
        }

        public void IncorrectValueAttackDefender(RequestBattle requestBattle) 
        {
            if (requestBattle.atacante == 0 || requestBattle.defensor == 0) throw new IncorrectValuesExcepion("El atacante o el defensor tienen valores incorrectos");
        }

        public void IncorrectValue(RequestBattle requestBattle)
        {
            if (requestBattle.ataque <= 0 || requestBattle.ataque > 100 || requestBattle.defensa <= 0 || requestBattle.defensa > 100)
            {
                throw new IncorrectValuesExcepion("El ataque o la defensa contiene un valor incorrecto");
            }
        }

    }
}
