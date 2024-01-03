using System;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace LethalAPI.LibTerminal.Helpers
{
    internal static class MiscHelper
    {
        //public void CloneUIElement(GameObject original, Action<GameObject> onCloneCreated, string name = null)
        //{
        //    // Ensure the original GameObject is a UI element with a RectTransform
        //    RectTransform rectTransform = original.GetComponent<RectTransform>();
        //    if (rectTransform == null)
        //    {
        //        onCloneCreated?.Invoke(null);
        //        return;
        //    }

        //    // Asynchronously clone the object on the main thread using MainThreadDispatcher
        //    MainThreadDispatcher.Enqueue(() =>
        //    {
        //        // Clone the object using Unity's Instantiate method
        //        GameObject clone = UnityEngine.Object.Instantiate(original) as GameObject;

        //        // If the clone was successful, set the parent and potentially adjust other properties
        //        if (clone != null)
        //        {
        //            clone.transform.SetParent(original.transform.parent, false);
        //            clone.transform.localPosition = rectTransform.localPosition;
        //            clone.transform.localScale = rectTransform.localScale;
        //            clone.name = string.IsNullOrEmpty(name) ? original.name + "_Clone" : name;
        //        }

        //        // Invoke the callback with the cloned object
        //        onCloneCreated?.Invoke(clone);
        //    });
        //}

        public static string Buffer(string input)
        {
            var buffer = new StringBuilder();
            buffer.AppendLine();
            buffer.AppendLine();
            buffer.AppendLine();
            buffer.AppendLine(input);
            Console.WriteLine(buffer.ToString());
            return buffer.ToString();
        }

        public static bool IsGameStarted()
        {
            StartMatchLever lever = GameObject.FindFirstObjectByType<StartMatchLever>();
            return lever.leverHasBeenPulled;
        }

        public static string GetObjectProperties(object obj)
        {
            if (obj == null)
            {
                return "The provided object is null.";
            }

            StringBuilder sb = new StringBuilder();
            Type objectType = obj.GetType();

            sb.AppendLine($"Properties of {objectType.Name}:");
            foreach (PropertyInfo property in objectType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try
                {
                    // Get the value of the property.
                    var value = property.GetValue(obj, null);

                    // Write the name and value of the property to the StringBuilder.
                    sb.AppendLine($"{property.Name}: {value}");
                }
                catch (Exception ex)
                {
                    // If there's an error accessing the property, write that out instead.
                    sb.AppendLine($"{property.Name}: Could not read value ({ex.Message})");
                }
            }

            // Return the string with all the property names and their values.
            return sb.ToString();
        }
    }
}
