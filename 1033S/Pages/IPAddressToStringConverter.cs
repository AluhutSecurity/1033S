using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace _1033S.Pages {
    public class IPAddressToStringConverter : ValidationRule, IValueConverter {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture ) {
            if ( value == null ) return null;

            if ( value.GetType() == typeof( System.Net.IPAddress ) ) return ( ( System.Net.IPAddress ) value ).ToString();
            return String.Empty;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) {
            if ( value == null ) return null;

            if ( value.GetType() == typeof( string ) ) {
                System.Net.IPAddress addr;
                if ( System.Net.IPAddress.TryParse( ( string ) value, out addr ) ) {
                    return addr;
                }
            }

            return null;
        }

        public override ValidationResult Validate( object value, CultureInfo cultureInfo ) {
            System.Net.IPAddress addr;
            if ( System.Net.IPAddress.TryParse( ( string ) value, out addr ) ) {
                return new ValidationResult( true, null );
            } else {
                return new ValidationResult(false, "please specify an valid ipaddress" );
            }
        }
    }
}
