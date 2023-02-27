// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using interop;
using Microsoft.PowerToys.Run.Plugin.PowerToysCommands.Properties;
using Wox.Plugin;

namespace Microsoft.PowerToys.Run.Plugin.PowerToysCommands
{
    public class ResultHelper
    {
        private enum Command
        {
            ColorPicker,
            FancyZonesEditor,
            MeasureTool,
            PowerOCR,
            ShortcutGuide,
        }

        public ResultHelper()
        {
            AllCommandResults = new List<Result>
            {
                CreateResult(
                    Resources.CommandColorPickerTitle,
                    "images/ColorPicker.png",
                    Resources.CommandColorPickerSubTitle,
                    Command.ColorPicker),
                CreateResult(
                    Resources.CommandFancyZonesEditorTitle,
                    "images/FancyZones.png",
                    Resources.CommandFancyZonesEditorSubTitle,
                    Command.FancyZonesEditor),
                CreateResult(
                    Resources.CommandMeasureToolTitle,
                    "images/MeasureTool.png",
                    Resources.CommandMeasureToolSubTitle,
                    Command.MeasureTool),
                CreateResult(
                    Resources.CommandPowerOCRTitle,
                    "images/PowerOCR.png",
                    Resources.CommandPowerOCRSubTitle,
                    Command.PowerOCR),
                CreateResult(
                    Resources.CommandShortcutGuideTitle,
                    "images/ShortcutGuide.png",
                    Resources.CommandShortcutGuideSubTitle,
                    Command.ShortcutGuide),
            };
        }

        private Result CreateResult(string title, string iconPath, string subTitle, Command command)
        {
            return new Result
            {
                Title = title,
                IcoPath = iconPath,
                Score = 300,
                SubTitle = subTitle,
                Action = c => Action(command),
            };
        }

        public List<Result> AllCommandResults { get; private set; }

        public IEnumerable<Result> GetCommandsMatchingQuery(Query query)
        {
            var filteredResults = AllCommandResults.Where(
                commandResult => DoesResultMatchQuery(commandResult, query));
#if DEBUG
            {
                string queryText = "PowerToys Commands Query: '" + query.RawQuery + "' '" + query.Search + "'";
                string resultText = "PowerToys Commands Results: " + string.Join(' ', filteredResults.Select(result => result.Title));
                Console.WriteLine(queryText);
                Console.WriteLine(resultText);
            }
#endif
            return filteredResults;
        }

        private static bool DoesResultMatchQuery(Result result, Query query)
        {
            return result.Title.ToLower().Contains(query.Search.ToLower()) ||
                result.SubTitle.ToLower().Contains(query.Search.ToLower());
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
