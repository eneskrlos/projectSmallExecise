namespace PequennosEjerciciosApi.Controllers.Exceptions
{
    public class IncorrectValuesExcepion : Exception
    {
        public IncorrectValuesExcepion() { }
        public IncorrectValuesExcepion(string message) : base(message) { }
    }
}
