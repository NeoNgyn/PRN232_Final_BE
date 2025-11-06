namespace EzyFix.API.Constants;

public class EnumConstants
{
    public static class ErrCodes
    {
        public static class Appointment
        {
            public const string InvalidTransition = "APPOINTMENT_INVALID_TRANSITION";
            public const string UnauthorizedTechnician = "APPOINTMENT_UNAUTHORIZED_TECHNICIAN";
            public const string MissingGps = "APPOINTMENT_MISSING_GPS";
            public const string AlreadyTerminal = "APPOINTMENT_ALREADY_TERMINAL";
        }
    }
}
