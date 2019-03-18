using System;

namespace MMOServer.Exceptions
{
    class AccountAlreadyExistException : Exception
    {
        public AccountAlreadyExistException()
        {
        }

        public AccountAlreadyExistException(string message)
            : base(message)
        {            
        }

        public AccountAlreadyExistException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
