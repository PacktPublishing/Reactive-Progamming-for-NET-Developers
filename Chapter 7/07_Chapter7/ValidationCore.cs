using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _07_Chapter7
{
    public interface IValidable { }

    public interface IValidableObjectResult<T>
            where T : IValidable
    {
        bool IsValid { get; }
        IEnumerable<ValidationResult> Result { get; }
        T Instance { get; }
    }

    public sealed class ValidableObjectResult<T> : IValidableObjectResult<T>
            where T : IValidable
    {
        public bool IsValid { get; set; }
        public IEnumerable<ValidationResult> Result { get; set; }
        public T Instance { get; set; }
    }

    public static class ValidableObjectHelper
    {
        /// <summary>
        /// Validates the argument
        /// </summary>
        public static IValidableObjectResult<T> Validate<T>(T arg)
            where T : IValidable
        {
            var context = new ValidationContext(arg);
            var errors = new List<ValidationResult>();

            if (Validator.TryValidateObject(arg, context, errors))
                return new ValidableObjectResult<T>()
                {
                    Instance = arg,
                    IsValid = true,
                    Result = Enumerable.Empty<ValidationResult>(),
                };
            else
                return new ValidableObjectResult<T>()
                {
                    Instance = arg,
                    IsValid = false,
                    Result = errors.AsEnumerable(),
                };
        }
    }
}
