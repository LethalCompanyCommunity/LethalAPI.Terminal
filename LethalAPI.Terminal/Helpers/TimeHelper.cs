using System;


namespace LethalAPI.LibTerminal.Helpers
{
    internal static class TimeHelper
    {

        public static string GetTime()
        {
            if (MiscHelper.IsGameStarted())
            {
                var timeObject = TimeOfDay.Instance;
                var time = DecimalToTimeString(timeObject.normalizedTimeOfDay);
                return time;
            }
            return null;
        }

        public static string DecimalToTimeString(float time)
        {
            // Constants
            int baseHour = 8; // Starting from 8 AM
            int totalMinutesInPeriod = 18 * 60; // Total minutes from 8 AM to Midnight (16 hours)

            // Calculate the total minutes from the float
            int minutesFromBase = (int)(time * totalMinutesInPeriod);

            // Calculate the time
            DateTime baseTime = DateTime.Today.AddHours(baseHour); // Today at 8:00 AM
            DateTime newTime = baseTime.AddMinutes(minutesFromBase);
            newTime = newTime.AddHours(-2);

            // Format the time as a string in the desired format "h:mm tt" (e.g., "8:30 AM")
            return newTime.ToString("h:mm tt");
        }
    }
}
