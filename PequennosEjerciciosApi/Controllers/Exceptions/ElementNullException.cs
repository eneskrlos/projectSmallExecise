namespace PequennosEjerciciosApi.Controllers.Exceptions
{
    public class ElementNullException : Exception
    {
        public ElementNullException() { }
        public ElementNullException(string message) : base(message) { }
    }
}
