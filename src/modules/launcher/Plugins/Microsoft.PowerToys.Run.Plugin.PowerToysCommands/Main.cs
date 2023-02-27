// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ManagedCommon;
using Microsoft.PowerToys.Run.Plugin.PowerToysCommands.Properties;
using Microsoft.PowerToys.Settings.UI.Library;
using Wox.Plugin;

namespace Microsoft.PowerToys.Run.Plugin.PowerToysCommands
{
    public class Main : IPlugin, IPluginI18n
    {
        public void Init(PluginInitContext context)
        {
        }

        public string Name => Resources.PluginTitle;

        public string Description => Resources.PluginDescription;

        private ResultHelper _resultHelper = new ResultHelper();

        public List<Result> Query(Query query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(paramName: nameof(query));
            }

            // Happens if the user has only typed the action key so far
            if (string.IsNullOrEmpty(query.Search))
            {
                return new List<Result>();
            }

            return _resultHelper.GetCommandsMatchingQuery(query).ToList();
        }

        public string GetTranslatedPluginTitle() => Name;

        public string GetTranslatedPluginDescription() => Description;
    }
}
