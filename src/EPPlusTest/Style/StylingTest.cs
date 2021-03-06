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
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Drawing;

namespace EPPlusTest.Style
{
    [TestClass]
    public class StylingTest : TestBase
    {
        static ExcelPackage _pck;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            _pck = OpenPackage("Style.xlsx", true);
        }
        [ClassCleanup]
        public static void Cleanup()
        {
            _pck.Save();
            _pck.Dispose();
        }
        [TestMethod]
        public void VerifyColumnStyle()
        {
            _pck = OpenPackage("Style.xlsx");
            var ws=_pck.Workbook.Worksheets.Add("RangeStyle");
            LoadTestdata(ws, 100,2,2);

            ws.Row(3).Style.Fill.SetBackground(ExcelIndexedColor.Indexed5);
            ws.Column(3).Style.Fill.SetBackground(ExcelIndexedColor.Indexed7);
            ws.Column(7).Style.Fill.SetBackground(eThemeSchemeColor.Accent1);
            ws.Row(6).Style.Fill.SetBackground(ExcelIndexedColor.Indexed4);

            ws.Cells["C3,F3"].Style.Fill.SetBackground(Color.Red);
            ws.Cells["F3"].Style.Fill.SetBackground(Color.Red);
            ws.Cells["C2"].Value = 2;
            ws.Cells["A3"].Value = "A3";

            Assert.AreEqual(7, ws.Cells["C2"].Style.Fill.BackgroundColor.Indexed);
            Assert.AreEqual(eThemeSchemeColor.Accent1, ws.Cells["G2"].Style.Fill.BackgroundColor.Theme);

            Assert.AreEqual(5, ws.Cells["A3"].Style.Fill.BackgroundColor.Indexed);
            Assert.AreEqual(Color.Red.ToArgb().ToString("X"), ws.Cells["C3"].Style.Fill.BackgroundColor.Rgb);
            Assert.AreEqual(Color.Red.ToArgb().ToString("X"), ws.Cells["F3"].Style.Fill.BackgroundColor.Rgb);
            Assert.AreEqual(eThemeSchemeColor.Accent1, ws.Cells["G3"].Style.Fill.BackgroundColor.Theme);
            Assert.AreEqual(5, ws.Cells["H3"].Style.Fill.BackgroundColor.Indexed);

            Assert.AreEqual(1, ws.Cells["A4"].StyleID);
            Assert.AreEqual(0, ws.Cells["F4"].StyleID);

            Assert.AreEqual(4, ws.Cells["A6"].Style.Fill.BackgroundColor.Indexed);
            Assert.AreEqual(4, ws.Cells["F6"].Style.Fill.BackgroundColor.Indexed);
            Assert.AreEqual(4, ws.Cells["G6"].Style.Fill.BackgroundColor.Indexed);

            Assert.AreEqual(eThemeSchemeColor.Accent1, ws.Cells["G7"].Style.Fill.BackgroundColor.Theme);

            Assert.AreEqual(7, ws.Cells["C102"].Style.Fill.BackgroundColor.Indexed);

            _pck.Save();
        }
    }
}
