using Sitecore.Data.Validators;
using Sitecore.Data.Fields;
using System;
using System.Runtime.Serialization;

namespace Foundation.Validation.Validators
{
    public class FutureDateValidator : StandardValidator
    {
        public FutureDateValidator() : base()
        {
        }

        public FutureDateValidator(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        protected override ValidatorResult Evaluate()
        {
            Field field = this.GetField();
            DateTime time = Sitecore.DateUtil.IsoDateToDateTime(field.Value).Date;
            if (time >= DateTime.UtcNow)
            {
                return ValidatorResult.Valid;
            }
            this.Text = this.GetText("The field \"{0}\" should contain a future date", field.DisplayName);
            return ValidatorResult.CriticalError;
        }
        protected override ValidatorResult GetMaxValidatorResult() { return GetFailedResult(ValidatorResult.CriticalError); }
        public override string Name => "Date In Future Validator";
    }
}