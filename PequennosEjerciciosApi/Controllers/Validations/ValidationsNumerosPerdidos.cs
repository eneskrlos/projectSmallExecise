using PequennosEjerciciosApi.Controllers.Exceptions;

namespace PequennosEjerciciosApi.Controllers.Validations
{
    public class ValidationsNumerosPerdidos
    {
        public void EmptyListOrListSmall(List<int> numbers)
        {
            if (numbers.Count < 2 || numbers.Count == 0) throw new EmptyListOrListSmallException("La lista es vacia o no debe ser menor que dos");
        }

        public void ListNumberException(bool asc, int number1, int number2)
        {
            if (asc)
            {
                if (number1 >= number2)
                {
                    throw new ListNumberExceptionException("El listado no puede poseer numeros repetidos ni estar desordenados.");
                }

            }
            else
            {
                if (number1 <= number2)
                {
                    throw new ListNumberExceptionException("El listado no puede poseer numeros repetidos ni estar desordenados.");
                }
            }
        }
    }
}
