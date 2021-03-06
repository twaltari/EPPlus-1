/*************************************************************************************************
  Required Notice: Copyright (C) EPPlus Software AB. 
  This software is licensed under PolyForm Noncommercial License 1.0.0 
  and may only be used for noncommercial purposes 
  https://polyformproject.org/licenses/noncommercial/1.0.0/

  A commercial license to use this software can be purchased at https://epplussoftware.com
 *************************************************************************************************
  Date               Author                       Change
 *************************************************************************************************
  01/27/2020         EPPlus Software AB       Initial release EPPlus 5
 *************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using OfficeOpenXml.Utils;
using OfficeOpenXml.DataValidation.Formulas.Contracts;

namespace OfficeOpenXml.DataValidation.Formulas
{
    /// <summary>
    /// Enumeration representing the state of an <see cref="ExcelDataValidationFormulaValue{T}"/>
    /// </summary>
    internal enum FormulaState
    {
        /// <summary>
        /// Value is set
        /// </summary>
        Value,
        /// <summary>
        /// Formula is set
        /// </summary>
        Formula
    }

    /// <summary>
    /// Base class for a formula
    /// </summary>
    internal abstract class ExcelDataValidationFormula : XmlHelper
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="namespaceManager">Namespacemanger of the worksheet</param>
        /// <param name="topNode">validation top node</param>
        /// <param name="formulaPath">xml path of the current formula</param>
        public ExcelDataValidationFormula(XmlNamespaceManager namespaceManager, XmlNode topNode, string formulaPath)
            : base(namespaceManager, topNode)
        {
            Require.Argument(formulaPath).IsNotNullOrEmpty("formulaPath");
            FormulaPath = formulaPath;
        }

        private string _formula;

        protected string FormulaPath
        {
            get;
            private set;
        }

        /// <summary>
        /// State of the validationformula, i.e. tells if value or formula is set
        /// </summary>
        protected FormulaState State
        {
            get;
            set;
        }

        /// <summary>
        /// A formula which output must match the current validation type
        /// </summary>
        public string ExcelFormula
        {
            get
            {
                return _formula;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    ResetValue();
                    State = FormulaState.Formula;
                }
                if (value != null && value.Length > 255)
                {
                    throw new InvalidOperationException("The length of a DataValidation formula cannot exceed 255 characters");
                }
                //var val = SqRefUtility.ToSqRefAddress(value);
                _formula = value;
                SetXmlNodeString(FormulaPath, value);
            }
        }

        internal abstract void ResetValue();

        /// <summary>
        /// This value will be stored in the xml. Can be overridden by subclasses
        /// </summary>
        internal virtual string GetXmlValue()
        {
            if (State == FormulaState.Formula)
            {
                return ExcelFormula;
            }
            return GetValueAsString();
        }

        /// <summary>
        /// Returns the value as a string. Must be implemented by subclasses
        /// </summary>
        /// <returns></returns>
        protected abstract string GetValueAsString();
    }
}
