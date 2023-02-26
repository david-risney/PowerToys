// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Threading;
using System.Windows;
using interop;
using Microsoft.PowerToys.Settings.UI.Library;
using Microsoft.PowerToys.Settings.UI.Views;
using Wox.Plugin;

namespace Microsoft.PowerToys.Run.Plugin.PowerToysCommands
{
    public class ResultHelper
    {
        public ResultHelper()
        {
            AllCommands = new List<Result>
            {
                CreateResult(
                    "PowerToys ColorPicker",
                    "Start PowerToys ColorPicker",
                    Command.ColorPicker),
                CreateResult(
                    "PowerToys FancyZones Editor",
                    "Start PowerToys Fancy Zones Editor",
                    Command.FancyZonesEditor),
                CreateResult(
                    "PowerToys MeasureTool",
                    "Start PowerToys MeasureTool",
                    Command.MeasureTool),
                CreateResult(
                    "PowerToys PowerOCR",
                    "Start PowerToys PowerOCR",
                    Command.PowerOCR),
                CreateResult(
                    "PowerToys Shortcut Guide",
                    "Start PowerToys Shortcut Guide",
                    Command.ShortcutGuide),
            };
        }

        private enum Command
        {
            ColorPicker,
            FancyZonesEditor,
            MeasureTool,
            PowerOCR,
            ShortcutGuide,
        }

        private Result CreateResult(string title, string subTitle, Command command)
        {
            return new Result
            {
                // Using CurrentCulture since this is user facing
                Title = title,
                IcoPath = IconPath,
                Score = 300,
                SubTitle = subTitle,
                Action = c => Action(command),
            };
        }

        public string IconPath { get; set; }

        public List<Result> AllCommands { get; private set; }

        public IEnumerable<Result> GetCommandsMatchingQuery(Query query)
        {
            return AllCommands.Where(
                commandResult => DoesResultMatchQuery(commandResult, query));
        }

        private static bool DoesResultMatchQuery(Result result, Query query)
        {
            return result.Title.ToLower().Contains(query.Search) ||
                result.SubTitle.ToLower().Contains(query.Search);
        }

        private static bool SetEvent(string eventName)
        {
            using (var eventHandle = new EventWaitHandle(false, EventResetMode.AutoReset, eventName))
            {
                eventHandle.Set();
            }

            return true;
        }

        private static bool Action(Command command)
        {
            var ret = false;

            switch (command)
            {
                case Command.ColorPicker: ret = SetEvent(Constants.ShowColorPickerSharedEvent()); break;
                case Command.FancyZonesEditor: ret = SetEvent(Constants.FZEToggleEvent()); break;
                case Command.MeasureTool: ret = SetEvent(Constants.MeasureToolTriggerEvent()); break;
                case Command.PowerOCR: ret = SetEvent(Constants.ShowPowerOCRSharedEvent()); break;
                case Command.ShortcutGuide: ret = SetEvent(Constants.ShortcutGuideTriggerEvent()); break;
                default: MessageBox.Show("Error: PowerToysCommands Command enum not implemented " + command); break;
            }

            return ret;
        }
    }
}
