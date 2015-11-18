using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace _1033S.Pages {
    public class PortValidationRule : ValidationRule {
        public override ValidationResult Validate( object value, CultureInfo cultureInfo ) {
            ushort p;
            if(ushort.TryParse(( string )value, out p)) {
                if(p >= _1033C.Defines.LOWESTVALIDPORT && p <= _1033C.Defines.HIGHESTVALIDPORT ) {
                    return new ValidationResult( true, null );
                } else {
                    return new ValidationResult( false, "port must be greater or equal to " + _1033C.Defines.LOWESTVALIDPORT + " and less or equal to " + _1033C.Defines.HIGHESTVALIDPORT );
                }
            }
            return new ValidationResult( false, "please enter a value between " + _1033C.Defines.LOWESTVALIDPORT + " and " + _1033C.Defines.HIGHESTVALIDPORT );
        }
    }
}
