using System;
using System.Configuration;

namespace FileDistributorService.Configuration.Validation
{
    internal class SkipDefaultValueCallbackValidator : ConfigurationValidatorBase
    {
        Type _type;
        ValidatorCallback _callback;
        private bool _isDefaultValueCall = true;

        public SkipDefaultValueCallbackValidator(Type type, ValidatorCallback callback) : this(callback)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            _type = type;
        }

        // Do not check for null type here to handle the callback attribute case
        public SkipDefaultValueCallbackValidator(ValidatorCallback callback)
        {
            if (callback == null)
                throw new ArgumentNullException("callback");

            _type = null;
            _callback = callback;
        }

        public override bool CanValidate(Type type)
        {
            return (type == _type || _type == null);
        }

        public override void Validate(object value)
        {
            if (_isDefaultValueCall)
            {
                _isDefaultValueCall = false;
                return;
            }

            _callback(value);
        }
    }
}
