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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeOpenXml.FormulaParsing;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using FakeItEasy;

namespace EPPlusTest.FormulaParsing.LexicalAnalysis
{
    [TestClass]
    public class TokenFactoryTests
    {
        private ITokenFactory _tokenFactory;
        private INameValueProvider _nameValueProvider;


        [TestInitialize]
        public void Setup()
        {
            var context = ParsingContext.Create();
            var excelDataProvider = A.Fake<ExcelDataProvider>();
            _nameValueProvider = A.Fake<INameValueProvider>();
            _tokenFactory = new TokenFactory(context.Configuration.FunctionRepository, _nameValueProvider);
        }

        [TestCleanup]
        public void Cleanup()
        {
      
        }

        [TestMethod]
        public void ShouldCreateAStringToken()
        {
            var input = "\"";
            var token = _tokenFactory.Create(Enumerable.Empty<Token>(), input);

            Assert.AreEqual("\"", token.Value);
            Assert.AreEqual(TokenType.String, token.TokenType);
        }

        [TestMethod]
        public void ShouldCreatePlusAsOperatorToken()
        {
            var input = "+";
            var token = _tokenFactory.Create(Enumerable.Empty<Token>(), input);

            Assert.AreEqual("+", token.Value);
            Assert.AreEqual(TokenType.Operator, token.TokenType);
        }

        [TestMethod]
        public void ShouldCreateMinusAsOperatorToken()
        {
            var input = "-";
            var token = _tokenFactory.Create(Enumerable.Empty<Token>(), input);

            Assert.AreEqual("-", token.Value);
            Assert.AreEqual(TokenType.Operator, token.TokenType);
        }

        [TestMethod]
        public void ShouldCreateMultiplyAsOperatorToken()
        {
            var input = "*";
            var token = _tokenFactory.Create(Enumerable.Empty<Token>(), input);

            Assert.AreEqual("*", token.Value);
            Assert.AreEqual(TokenType.Operator, token.TokenType);
        }

        [TestMethod]
        public void ShouldCreateDivideAsOperatorToken()
        {
            var input = "/";
            var token = _tokenFactory.Create(Enumerable.Empty<Token>(), input);

            Assert.AreEqual("/", token.Value);
            Assert.AreEqual(TokenType.Operator, token.TokenType);
        }

        [TestMethod]
        public void ShouldCreateEqualsAsOperatorToken()
        {
            var input = "=";
            var token = _tokenFactory.Create(Enumerable.Empty<Token>(), input);

            Assert.AreEqual("=", token.Value);
            Assert.AreEqual(TokenType.Operator, token.TokenType);
        }

        [TestMethod]
        public void ShouldCreateIntegerAsIntegerToken()
        {
            var input = "23";
            var token = _tokenFactory.Create(Enumerable.Empty<Token>(), input);

            Assert.AreEqual("23", token.Value);
            Assert.AreEqual(TokenType.Integer, token.TokenType);
        }

        [TestMethod]
        public void ShouldCreateBooleanAsBooleanToken()
        {
            var input = "true";
            var token = _tokenFactory.Create(Enumerable.Empty<Token>(), input);

            Assert.AreEqual("true", token.Value);
            Assert.AreEqual(TokenType.Boolean, token.TokenType);
        }

        [TestMethod]
        public void ShouldCreateDecimalAsDecimalToken()
        {
            var input = "23.3";
            var token = _tokenFactory.Create(Enumerable.Empty<Token>(), input);

            Assert.AreEqual("23.3", token.Value);
            Assert.AreEqual(TokenType.Decimal, token.TokenType);
        }

        [TestMethod]
        public void CreateShouldReadFunctionsFromFuncRepository()
        {
            var input = "Text";
            var token = _tokenFactory.Create(Enumerable.Empty<Token>(), input);
            Assert.AreEqual(TokenType.Function, token.TokenType);
            Assert.AreEqual("Text", token.Value);
        }

        [TestMethod]
        public void CreateShouldCreateExcelAddressAsExcelAddressToken()
        {
            var input = "A1";
            var token = _tokenFactory.Create(Enumerable.Empty<Token>(), input);
            Assert.AreEqual(TokenType.ExcelAddress, token.TokenType);
            Assert.AreEqual("A1", token.Value);
        }

        [TestMethod]
        public void CreateShouldCreateExcelRangeAsExcelAddressToken()
        {
            var input = "A1:B15";
            var token = _tokenFactory.Create(Enumerable.Empty<Token>(), input);
            Assert.AreEqual(TokenType.ExcelAddress, token.TokenType);
            Assert.AreEqual("A1:B15", token.Value);
        }

        [TestMethod]
        public void CreateShouldCreateExcelRangeOnOtherSheetAsExcelAddressToken()
        {
            var input = "ws!A1:B15";
            var token = _tokenFactory.Create(Enumerable.Empty<Token>(), input);
            Assert.AreEqual(TokenType.ExcelAddress, token.TokenType);
            Assert.AreEqual("WS!A1:B15", token.Value);
        }

        [TestMethod]
        public void CreateShouldCreateNamedValueAsExcelAddressToken()
        {
            var input = "NamedValue";
            A.CallTo(() => _nameValueProvider.IsNamedValue("NamedValue","")).Returns(true);
            A.CallTo(() => _nameValueProvider.IsNamedValue("NamedValue", null)).Returns(true);
            var token = _tokenFactory.Create(Enumerable.Empty<Token>(), input);
            Assert.AreEqual(TokenType.NameValue, token.TokenType);
            Assert.AreEqual("NamedValue", token.Value);
        }
    }
}
