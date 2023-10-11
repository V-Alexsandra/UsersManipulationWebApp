namespace UsersManipulation.Business.Exceptions
{
    public class NotSucceededException : Exception
    {
        public NotSucceededException() { }

        public NotSucceededException(string message) : base(message) { }
    }
}
