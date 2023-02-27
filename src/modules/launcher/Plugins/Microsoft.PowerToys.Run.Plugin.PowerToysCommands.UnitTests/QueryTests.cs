// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Microsoft.PowerToys.Run.Plugin.PowerToysCommands.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Wox.Infrastructure;
using Wox.Plugin;

namespace Microsoft.PowerToys.Run.Plugin.PowerToysCommands.UnitTests
{
    [TestClass]
    public class QueryTests
    {
        [TestMethod]
        public void CommandEnumsMatchIndex()
        {
            // This isn't a DataTestMethod because Resources can't be used in an attribute.
            Tuple<CommandResults.Command, string>[] expected =
            {
                new(CommandResults.Command.ColorPicker, Resources.CommandColorPickerTitle),
                new(CommandResults.Command.ShortcutGuide, Resources.CommandShortcutGuideTitle),
            };
            var commandResults = new CommandResults();
            foreach (var expectedItem in expected)
            {
                Assert.AreEqual(
                    commandResults.AllCommandResults[(int)expectedItem.Item1].Title,
                    expectedItem.Item2);
            }
        }

        [TestMethod]
        public void CommandQueryMatchAll()
        {
            var commandResults = new CommandResults();
            var expectedResults = commandResults.AllCommandResults.ToList();
            var actualResults = commandResults.GetCommandsMatchingQuery(new Query("PowerToys")).ToList();

            // Assert that the expectedResults and actualResults lists contain the same things
            CollectionAssert.AreEquivalent(expectedResults, actualResults);
        }

        [TestMethod]
        public void CommandQueryMatchExact()
        {
            var commandResults = new CommandResults();

            // Run a query for Color Picker and make sure we get exactly one result that matches
            var actualResults = commandResults.GetCommandsMatchingQuery(new Query("PowerToys Color Picker")).ToList();
            Assert.AreEqual(1, actualResults.Count);
            Assert.AreEqual(Resources.CommandColorPickerTitle, actualResults[0].Title);
        }

        [TestMethod]
        public void CommandQueryMatchCaseInsensitive()
        {
            var commandResults = new CommandResults();

            // Run a query for Color Picker and make sure we get exactly one result that matches
            var actualResults = commandResults.GetCommandsMatchingQuery(new Query("pOwertOys cOlor pIcker")).ToList();
            Assert.AreEqual(1, actualResults.Count);
            Assert.AreEqual(Resources.CommandColorPickerTitle, actualResults[0].Title);
        }

        [TestMethod]
        public void CommandQueryMatchPartial()
        {
            var commandResults = new CommandResults();

            // Run a query for Color Picker and make sure we get exactly one result that matches
            var actualResults = commandResults.GetCommandsMatchingQuery(new Query("olor Picke")).ToList();
            Assert.AreEqual(1, actualResults.Count);
            Assert.AreEqual(Resources.CommandColorPickerTitle, actualResults[0].Title);
        }
    }
}
