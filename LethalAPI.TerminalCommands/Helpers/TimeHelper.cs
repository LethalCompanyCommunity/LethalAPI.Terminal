using System;
using System.Threading;
using TMPro;
using UnityEngine;

namespace LethalAPI.TerminalCommands.Helpers
{
    internal class TimeHelper
    {
        private Timer updateTimer;
        private MiscHelper miscHelper;

        public TimeHelper()
        {
            miscHelper = new MiscHelper();
            StartUpdating();
        }

        public void StartUpdating()
        {
            Console.WriteLine("StartUpdating...");
            //TerminalTime(null);
            if(updateTimer == null)
            {
                updateTimer = new Timer(TerminalTime, null, 0, 5000);
            }
        }

        public void StopUpdating()
        {
            Console.WriteLine("StopUpdating...");
            if (updateTimer != null)
            {
                updateTimer.Change(Timeout.Infinite, Timeout.Infinite);
                updateTimer.Dispose();
            }
        }

        public void TerminalTime(object state)
        {
            var time = GetTime();
            var terminalTime = GameObject.Find("TerminalTime");
            if (!string.IsNullOrEmpty(time) && terminalTime == null)
            {
                var originalObject = GameObject.Find("CurrentCreditsNum");
                if (originalObject != null)
                {
                    miscHelper.CloneUIElement(originalObject, (clone) =>
                    {
                        if (clone != null)
                        {
                            clone.transform.localPosition = new Vector3(259.14f, 208.9149f);
                            clone.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

                            var textMesh = clone.GetComponent<TextMeshProUGUI>();

                            if (textMesh != null)
                            {
                                textMesh.text = time;
                            }
                        }
                        else
                        {
                        }
                    }, "TerminalTime");

                }
            }
            else if (!string.IsNullOrEmpty(time) && terminalTime != null) 
            {
                TextMeshProUGUI textMesh = terminalTime.GetComponent<TextMeshProUGUI>();

                if (textMesh != null)
                {
                    textMesh.text = time;
                }

                terminalTime.SetActive(true);
            }
            else if(string.IsNullOrEmpty(time) && terminalTime != null)
            {
                terminalTime.SetActive(false);
            }
        }

        public string GetTime()
        {
            if (miscHelper.IsGameStarted())
            {
                var timeObject = TimeOfDay.Instance;
                var time = DecimalToTimeString(timeObject.normalizedTimeOfDay);
                return time;
            }
            return null;
        }

        public string DecimalToTimeString(float time)
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
