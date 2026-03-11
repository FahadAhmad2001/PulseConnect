using System;
using System.Collections.Generic;
using System.Text;

namespace PulseConnectLib.Definitions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException() { }
        public UserAlreadyExistsException(string message) : base(message) { }
        public UserAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }

    }

    public class UserDoesNotExistException : Exception
    {
        public UserDoesNotExistException() { }
        public UserDoesNotExistException(string message) : base(message) { }
        public UserDoesNotExistException(string message, Exception innerException) : base(message, innerException) { }

    }
    public class UserWrongPasswordException : Exception
    {
        public UserWrongPasswordException() { }
        public UserWrongPasswordException(string message) : base(message) { }
        public UserWrongPasswordException(string message, Exception innerException) : base(message, innerException) { }

    }
    public class UserExpiredCookieException : Exception
    {
        public UserExpiredCookieException() { }
        public UserExpiredCookieException(string message) : base(message) { }
        public UserExpiredCookieException(string message, Exception innerException) : base(message, innerException) { }

    }
}
