using System;
using System.Globalization;
using System.Web.Mvc;

namespace Agnus.Interface.Web.App_Start
{
    public class LongNullBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
       ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                if (valueResult != null && valueResult.AttemptedValue != "0" && !string.IsNullOrEmpty(valueResult.AttemptedValue))
                    actualValue = Convert.ToInt64(valueResult.AttemptedValue.Trim(','));
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
}