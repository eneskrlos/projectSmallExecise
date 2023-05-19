namespace PequennosEjerciciosApi.Controllers.Exceptions
{
    public class EmptyListOrListSmallException : Exception
    {

        public EmptyListOrListSmallException() { }

        public EmptyListOrListSmallException(string message) :base(message) { }
    }
}
