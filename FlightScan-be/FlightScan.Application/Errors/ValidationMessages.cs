namespace FlightScan.Application.Errors
{
    public class ValidationMessages
    {
        // CreateUserCommand messages 
        public const string usernameRequired = "Username is required.";
        public const string usernameMinChars = "Username must be at least 8 characters.";
        public const string usernameMaxChars = "Username cannot exceed 50 characters.";
        public const string usernameExists = "User with provided username already exist.";
        public const string passwordRequired = "Password is required.";
        public const string passwordMinChars = "Password must be at least 6 characters.";
        public const string passwordMaxChars = "Password cannot exceed 50 characters.";
        public const string roleInvalid = "Invalid role provided.";
        public const string administratorRoleInvalid = "Cannot create user with administrator role.";

        // CancelFlightCommand messages
        public const string flightIdInvalid = "Invalid flight ID.";
        public const string flightNotFound = "Flight with provided ID does not exist.";

    }
}
