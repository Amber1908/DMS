using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.DataAnnotaionExt
{
    public class MaxFileNameAttribute : ValidationAttribute
    {
        private int _maxlength;

        public MaxFileNameAttribute(int maxlength)
        {
            this._maxlength = maxlength;
            ErrorMessage = "{0} 的檔案名稱字數不得超過 {1}";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string filename = Path.GetFileName((string)value);
            if (filename.Length < _maxlength)
            {
                return ValidationResult.Success;
            }
            else
            {
                string errorMessage = string.Format(ErrorMessage, validationContext.DisplayName, _maxlength);
                return new ValidationResult(errorMessage);
            }
        }
    }
}
