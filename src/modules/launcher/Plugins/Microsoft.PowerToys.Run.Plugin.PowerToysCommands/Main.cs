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
    public class Main : IPlugin, IPluginI18n, IDisposable, ISettingProvider
    {
        private PluginInitContext Context { get; set; }

        public string Name => "PowerToysCommands";

        public string Description => "Run PowerToys commands from PowerToys Launcher";

        private bool _disposed;
        private ResultHelper _resultHelper = new ResultHelper();

        public IEnumerable<PluginAdditionalOption> AdditionalOptions => new List<PluginAdditionalOption>()
        {
        };

        public List<Result> Query(Query query)
        {
            bool isGlobalQuery = string.IsNullOrEmpty(query.ActionKeyword);

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

        public void Init(PluginInitContext context)
        {
            Context = context ?? throw new ArgumentNullException(paramName: nameof(context));

            Context.API.ThemeChanged += OnThemeChanged;
            UpdateIconPath(Context.API.GetCurrentTheme());
        }

        private void UpdateIconPath(Theme theme)
        {
            string path = "Images/icon.dark.png";
            if (theme == Theme.Light || theme == Theme.HighContrastWhite)
            {
                path = "Images/icon.light.png";
            }

            _resultHelper.IconPath = path;
        }

        private void OnThemeChanged(Theme currentTheme, Theme newTheme)
        {
            UpdateIconPath(newTheme);
        }

        public string GetTranslatedPluginTitle()
        {
            return Name;
        }

        public string GetTranslatedPluginDescription()
        {
            return Description;
        }

        public Control CreateSettingPanel()
        {
            throw new NotImplementedException();
        }

        public void UpdateSettings(PowerLauncherPluginSettings settings)
        {
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (Context != null && Context.API != null)
                    {
                        Context.API.ThemeChanged -= OnThemeChanged;
                    }

                    _disposed = true;
                }
            }
        }
    }
}
