using System.Globalization;
using Kwops.Mobile.Models;

namespace Kwops.Mobile.Converters;

class DevelopersConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var developers = value as IEnumerable<Developer>;
        if (developers == null)
        {
            return string.Empty;
        }

        string developersAsText = string.Join(", ",
            developers.Select(d => $"{d.FirstName} {d.LastName} ({(int)(d.Rating * 100)}%)"));

        if (string.IsNullOrEmpty(developersAsText))
        {
            return "No developers in this team yet...";
        }

        return developersAsText;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        //TODO
        throw new NotImplementedException();
    }
}