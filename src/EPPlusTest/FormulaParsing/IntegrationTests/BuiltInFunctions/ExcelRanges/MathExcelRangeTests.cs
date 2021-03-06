/*******************************************************************************
 * You may amend and distribute as you like, but don't remove this header!
 *
 * Required Notice: Copyright (C) EPPlus Software AB. 
 * https://epplussoftware.com
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.

 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
 * See the GNU Lesser General Public License for more details.
 *
 * The GNU Lesser General Public License can be viewed at http://www.opensource.org/licenses/lgpl-license.php
 * If you unfamiliar with this license or have questions about it, here is an http://www.gnu.org/licenses/gpl-faq.html
 *
 * All code and executables are provided "" as is "" with no warranty either express or implied. 
 * The author accepts no liability for any damage or loss of business that this product may cause.
 *
 * Code change notes:
 * 
  Date               Author                       Change
 *******************************************************************************
  01/27/2020         EPPlus Software AB       Initial release EPPlus 5
 *******************************************************************************/
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeOpenXml;

namespace EPPlusTest.FormulaParsing.IntegrationTests.BuiltInFunctions.ExcelRanges
{
    [TestClass]
    public class MathExcelRangeTests
    {
        private ExcelPackage _package;
        private ExcelWorksheet _worksheet;

        [TestInitialize]
        public void Initialize()
        {
            _package = new ExcelPackage();
            _worksheet = _package.Workbook.Worksheets.Add("Test");

            _worksheet.Cells["A1"].Value = 1;
            _worksheet.Cells["A2"].Value = 3;
            _worksheet.Cells["A3"].Value = 6;
        }

        [TestCleanup]
        public void Cleanup()
        {
            _package.Dispose();
        }

        [TestMethod]
        public void AbsShouldReturn3()
        {
            _worksheet.Cells["A4"].Formula = "ABS(A2)";
            _worksheet.Calculate();
            var result = _worksheet.Cells["A4"].Value;
            Assert.AreEqual(3d, result);
        }

        [TestMethod]
        public void CountShouldReturn3()
        {
            _worksheet.Cells["A4"].Formula = "COUNT(A1:A3)";
            _worksheet.Calculate();
            var result = _worksheet.Cells["A4"].Value;
            Assert.AreEqual(3d, result);
        }

        [TestMethod]
        public void CountShouldReturn2IfACellValueIsNull()
        {
            _worksheet.Cells["A2"].Value = null;
            _worksheet.Cells["A4"].Formula = "COUNT(A1:A3)";
            _worksheet.Calculate();
            var result = _worksheet.Cells["A4"].Value;
            Assert.AreEqual(2d, result);
        }

        [TestMethod]
        public void CountAShouldReturn3()
        {
            _worksheet.Cells["A4"].Formula = "COUNTA(A1:A3)";
            _worksheet.Calculate();
            var result = _worksheet.Cells["A4"].Value;
            Assert.AreEqual(3d, result);
        }

        [TestMethod]
        public void CountIfShouldReturnCorrectResult()
        {
            _worksheet.Cells["A4"].Formula = "COUNTIF(A1:A3, \">2\")";
            _worksheet.Calculate();
            var result = _worksheet.Cells["A4"].Value;
            Assert.AreEqual(2d, result);
        }

        [TestMethod]
        public void MaxShouldReturn6()
        {
            _worksheet.Cells["A4"].Formula = "Max(A1:A3)";
            _worksheet.Calculate();
            var result = _worksheet.Cells["A4"].Value;
            Assert.AreEqual(6d, result);
        }

        [TestMethod]
        public void MinShouldReturn1()
        {
            _worksheet.Cells["A4"].Formula = "Min(A1:A3)";
            _worksheet.Calculate();
            var result = _worksheet.Cells["A4"].Value;
            Assert.AreEqual(1d, result);
        }

        [TestMethod]
        public void AverageShouldReturn3Point333333()
        {
            _worksheet.Cells["A4"].Formula = "Average(A1:A3)";
            _worksheet.Calculate();
            var result = _worksheet.Cells["A4"].Value;
            Assert.AreEqual(3d + (1d/3d), result);
        }

        [TestMethod]
        public void AverageIfShouldHandleSingleRangeNumericExpressionMatch()
        {
            _worksheet.Cells["A4"].Value = "B";
            _worksheet.Cells["A5"].Value = 3;
            _worksheet.Cells["A6"].Formula = "AverageIf(A1:A5,\">1\")";
            _worksheet.Calculate();
            Assert.AreEqual(4d, _worksheet.Cells["A6"].Value);
        }

        [TestMethod]
        public void AverageIfShouldHandleSingleRangeStringMatch()
        {
            _worksheet.Cells["A4"].Value = "ABC";
            _worksheet.Cells["A5"].Value = "3";
            _worksheet.Cells["A6"].Formula = "AverageIf(A1:A5,\">1\")";
            _worksheet.Calculate();
            Assert.AreEqual(4.5d, _worksheet.Cells["A6"].Value);
        }

        [TestMethod]
        public void AverageIfShouldHandleLookupRangeStringMatch()
        {
            _worksheet.Cells["A1"].Value = "abc";
            _worksheet.Cells["A2"].Value = "abc";
            _worksheet.Cells["A3"].Value = "def";
            _worksheet.Cells["A4"].Value = "def";
            _worksheet.Cells["A5"].Value = "abd";

            _worksheet.Cells["B1"].Value = 1;
            _worksheet.Cells["B2"].Value = 3;
            _worksheet.Cells["B3"].Value = 5;
            _worksheet.Cells["B4"].Value = 6;
            _worksheet.Cells["B5"].Value = 7;

            _worksheet.Cells["A6"].Formula = "AverageIf(A1:A5,\"abc\",B1:B5)";
            _worksheet.Calculate();
            Assert.AreEqual(2d, _worksheet.Cells["A6"].Value);
        }

        [TestMethod]
        public void AverageIfShouldHandleLookupRangeStringNumericMatch()
        {
            _worksheet.Cells["A1"].Value = 1;
            _worksheet.Cells["A2"].Value = 3;
            _worksheet.Cells["A3"].Value = 3;
            _worksheet.Cells["A4"].Value = 5;
            _worksheet.Cells["A5"].Value = 2;

            _worksheet.Cells["B1"].Value = 3;
            _worksheet.Cells["B2"].Value = 3;
            _worksheet.Cells["B3"].Value = 2;
            _worksheet.Cells["B4"].Value = 1;
            _worksheet.Cells["B5"].Value = 8;

            _worksheet.Cells["A6"].Formula = "AverageIf(A1:A5,\">2\",B1:B5)";
            _worksheet.Calculate();
            Assert.AreEqual(2d, _worksheet.Cells["A6"].Value);
        }

        [TestMethod]
        public void AverageIfShouldHandleLookupRangeStringWildCardMatch()
        {
            _worksheet.Cells["A1"].Value = "abc";
            _worksheet.Cells["A2"].Value = "abc";
            _worksheet.Cells["A3"].Value = "def";
            _worksheet.Cells["A4"].Value = "def";
            _worksheet.Cells["A5"].Value = "abd";

            _worksheet.Cells["B1"].Value = 1;
            _worksheet.Cells["B2"].Value = 3;
            _worksheet.Cells["B3"].Value = 5;
            _worksheet.Cells["B4"].Value = 6;
            _worksheet.Cells["B5"].Value = 8;

            _worksheet.Cells["A6"].Formula = "AverageIf(A1:A5, \"ab*\",B1:B5)";
            _worksheet.Calculate();
            Assert.AreEqual(4d, _worksheet.Cells["A6"].Value);
        }

        [TestMethod]
        public void SumProductWithRange()
        {
            _worksheet.Cells["A1"].Value = 1;
            _worksheet.Cells["A2"].Value = 2;
            _worksheet.Cells["A3"].Value = 3;
            _worksheet.Cells["B1"].Value = 5;
            _worksheet.Cells["B2"].Value = 6;
            _worksheet.Cells["B3"].Value = 4;
            _worksheet.Cells["A4"].Formula = "SUMPRODUCT(A1:A3,B1:B3)";
            _worksheet.Calculate();
            var result = _worksheet.Cells["A4"].Value;
            Assert.AreEqual(29d, result);
        }

        [TestMethod]
        public void SumProductWithRangeAndValues()
        {
            _worksheet.Cells["A1"].Value = 1;
            _worksheet.Cells["A2"].Value = 2;
            _worksheet.Cells["A3"].Value = 3;
            _worksheet.Cells["B1"].Value = 5;
            _worksheet.Cells["B2"].Value = 6;
            _worksheet.Cells["B3"].Value = 4;
            _worksheet.Cells["A4"].Formula = "SUMPRODUCT(A1:A3,B1:B3,{2,4,1})";
            _worksheet.Calculate();
            var result = _worksheet.Cells["A4"].Value;
            Assert.AreEqual(70d, result);
        }

        [TestMethod]
        public void SignShouldReturn1WhenRefIsPositive()
        {
            _worksheet.Cells["A4"].Formula = "SIGN(A1)";
            _worksheet.Calculate();
            var result = _worksheet.Cells["A4"].Value;
            Assert.AreEqual(1d, result);
        }

        [TestMethod]
        public void SubTotalShouldNotIncludeHiddenRow()
        {
            _worksheet.Row(2).Hidden = true;
            _worksheet.Cells["A4"].Formula = "SUBTOTAL(109,A1:A3)";
            _worksheet.Calculate();
            var result = _worksheet.Cells["A4"].Value;
            Assert.AreEqual(7d, result);
        }

        [TestMethod]
        public void SumProductShouldWorkWithSingleCellArray()
        {
            _worksheet.Cells["A1"].Value = 1;
            _worksheet.Cells["A2"].Value = 2;
            _worksheet.Cells["A4"].Formula = "SUMPRODUCT(A1:A1, A2:A2)";
            _worksheet.Calculate();
            var result = _worksheet.Cells["A4"].Value;
            Assert.AreEqual(2d, result);
        }

        [TestMethod]
        public void ShouldIgnoreNullValues()
        {
            _worksheet.Cells["B3"].Formula = "C4 + D4";
            _worksheet.Calculate();
            var result = _worksheet.Cells["B3"].Value;
            Assert.AreEqual(0d, result);
        }
    }
}
