// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Microsoft.PowerToys.Run.Plugin.PowerToysCommands.Properties;
using Microsoft.PowerToys.Settings.UI.Library;
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
        public void CommandQueryMatches()
        {
            var commandResults = new CommandResults();
            Tuple<string, string>[] expected =
            {
                new Tuple<string, string>("PowerToys Color Picker", Resources.CommandColorPickerTitle),
                new Tuple<string, string>("pOwertOys cOlor pIcker", Resources.CommandColorPickerTitle),
                new Tuple<string, string>("olor Picke", Resources.CommandColorPickerTitle),
            };

            // Run a query for Color Picker and make sure we get exactly one result that matches
            foreach (var entry in expected)
            {
                var actualResults = commandResults.GetCommandsMatchingQuery(new Query(entry.Item1)).ToList();
                Assert.AreEqual(1, actualResults.Count);
                Assert.AreEqual(entry.Item2, actualResults[0].Title);
            }
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("unexpected input")]
        public void CommandQueryMatchNone(string queryText)
        {
            var commandResults = new CommandResults();
            var actualResults = commandResults.GetCommandsMatchingQuery(new Query(queryText)).ToList();
            Assert.AreEqual(0, actualResults.Count);
        }

        [TestMethod]
        public void CommandQueryThrowsOnNullQuery()
        {
            var commandResults = new CommandResults();
            Assert.ThrowsException<ArgumentNullException>(() => commandResults.GetCommandsMatchingQuery(null));
        }
    }
}
