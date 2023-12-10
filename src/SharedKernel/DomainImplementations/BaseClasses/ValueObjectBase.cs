﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedKernel.BusinessRules;
using SharedKernel.DomainImplementations.DomainErrors;

namespace SharedKernel.DomainImplementations.BaseClasses;

public abstract class ValueObjectBase
{
    private readonly List<BusinessRule> _brokenRules = new();

    public void ThrowExceptionIfInvalid()
    {
        _brokenRules.Clear();
        Validate();
        if (_brokenRules.Any())
        {
            var issues = new StringBuilder();
            foreach (var businessRule in _brokenRules)
                issues.AppendLine(businessRule.Rule);

            throw new ValueObjectIsInvalidException(issues.ToString());
        }
    }

    protected void AddBrokenRule(BusinessRule businessRule)
    {
        _brokenRules.Add(businessRule);
    }

    protected abstract void Validate();
}