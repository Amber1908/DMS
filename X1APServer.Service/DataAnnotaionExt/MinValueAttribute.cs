using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace X1APServer.Service.DataAnnotaionExt
{
    public class MinValueAttribute : ValidationAttribute
    {
        private int _minValue;

        public MinValueAttribute(int minValue)
        {
            _minValue = minValue;
            
            ErrorMessage = "{0} 數值不得小於 {1}";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || (int)value >= _minValue)
            {
                return ValidationResult.Success;
            }
            else
            {
                string errorMessage = string.Format(ErrorMessage, validationContext.DisplayName, _minValue);
                return new ValidationResult(errorMessage);
            }
        }
    }
}