using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ChatApp
{
    public class MessageStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Ensure the sender is a valid string
            if (value is not string sender)
            {
                return GetResource("ReceivedMessageStyle");
            }

            // Check for "Me" (case-insensitive for robustness)
            return sender.Equals("Me", StringComparison.OrdinalIgnoreCase) 
                ? GetResource("SentMessageStyle") 
                : GetResource("ReceivedMessageStyle");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        // Helper method to safely retrieve resources
        private object GetResource(string resourceKey)
        {
            return Application.Current.Resources.Contains(resourceKey) 
                ? Application.Current.Resources[resourceKey] 
                : new Style(); // Fallback to an empty style if the resource is missing
        }
    }
}
