﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace eTUTOR.Tests.Support
{
    public static class TestModelHelper
    {
        public static IList<ValidationResult> ValidateModel(this Controller controller, object viewModel)
        {
            controller.ModelState.Clear();
            var validationContext = new ValidationContext(viewModel, null, null);
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(viewModel, validationContext, validationResults, true);
            foreach(var result in validationResults)
            {
                foreach(var name in result.MemberNames)
                {
                    controller.ModelState.AddModelError(name, result.ErrorMessage);
                }
            }
            return validationResults;
        }
    }
}
