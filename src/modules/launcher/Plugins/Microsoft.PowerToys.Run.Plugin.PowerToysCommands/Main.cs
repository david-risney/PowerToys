// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.PowerToys.Run.Plugin.PowerToysCommands.Properties;
using Wox.Plugin;

namespace Microsoft.PowerToys.Run.Plugin.PowerToysCommands
{
    // This is the main class of the PowerToys Run plugin PowerToysCommands
    // which supports querying for PowerToys commands. See CommandResults
    // for where the command Results are actually created, created, and
    // invoked.
    public class Main : IPlugin, IPluginI18n
    {
        public void Init(PluginInitContext context)
        {
        }

        public string Name => Resources.PluginTitle;

        public string Description => Resources.PluginDescription;

        private static CommandResults _commandResults = new CommandResults();

        public List<Result> Query(Query query)
        {
            return _commandResults.GetCommandsMatchingQuery(query).ToList();
        }

        public string GetTranslatedPluginTitle() => Name;

        public string GetTranslatedPluginDescription() => Description;
    }
}
