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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EPPlusTest.Core.Range
{
    [TestClass]
    public class RangeTest
    {
        [TestMethod]
        public void ArrayToCellString()
        {
            var ms=new MemoryStream();
            using (var p = new ExcelPackage(ms))
            {
                var sheet = p.Workbook.Worksheets.Add("Sheet1");
                sheet.Cells[1, 1].Value = new[] { "string1", "string2", "string3" };
                p.Save();
            }
            using (var p = new ExcelPackage(ms))
            {
                var sheet = p.Workbook.Worksheets["Sheet1"];
                Assert.AreEqual("string1", sheet.Cells[1, 1].Value);
            }
        }

        [TestMethod]
        public void ArrayToCellNull()
        {
            var ms = new MemoryStream();
            using (var p = new ExcelPackage(ms))
            {
                var sheet = p.Workbook.Worksheets.Add("Sheet1");
                sheet.Cells[1, 1].Value = new[] { null, "string2", "string3" };
                p.Save();
            }
            using (var p = new ExcelPackage(ms))
            {
                var sheet = p.Workbook.Worksheets["Sheet1"];
                Assert.AreEqual(string.Empty, sheet.Cells[1, 1].Value);
            }
        }
        [TestMethod]
        public void ArrayToCellInt()
        {
            var ms = new MemoryStream();
            using (var p = new ExcelPackage(ms))
            {
                var sheet = p.Workbook.Worksheets.Add("Sheet1");
                sheet.Cells[1, 1].Value = new object[] { 1, "string2", "string3" };
                p.Save();
            }
            using (var p = new ExcelPackage(ms))
            {
                var sheet = p.Workbook.Worksheets["Sheet1"];
                Assert.AreEqual(1D, sheet.Cells[1, 1].Value);
            }
        }
    }
}
