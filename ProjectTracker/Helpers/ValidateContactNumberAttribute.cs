using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTracker.Helpers
{
    public class ValidateContactNumberAttribute : ValidationAttribute
    {
        private bool _forceValidation { get; set; }

        public ValidateContactNumberAttribute(string errorMessage, bool forceValidation = true)
        {
            this.ErrorMessage = errorMessage;
            this._forceValidation = forceValidation;
        }

        public override bool IsValid(object value)
        {
            //dont force if not required
            if (value == null && _forceValidation == false)
            {
                return true;
            }

            if (string.IsNullOrEmpty((string)value))
            {
                return false;
            }

            string cellPhone = value.ToString().Trim();

            if (string.IsNullOrEmpty(cellPhone) || cellPhone.Length < 10)
            {
                ErrorMessage = "Cellphone number must be a minimum of 10.";
                return false;
            }

            int start = 0;
            if (cellPhone.Substring(0, 1) == "+")
            {
                start = 1;
            }

            // ensure the number contains only digits from the second onwards
            for (var i = start; i < cellPhone.Length; i++)
                if (cellPhone[i] < '0' || cellPhone[i] > '9')
                {
                    ErrorMessage = "Cellphone number can start with a '+' but must only contain digits.";
                    return false;
                }

            return true;
        }
    }
}
