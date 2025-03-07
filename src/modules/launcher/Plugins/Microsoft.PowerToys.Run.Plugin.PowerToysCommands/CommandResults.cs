// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using interop;
using Microsoft.PowerToys.Run.Plugin.PowerToysCommands.Properties;
using Wox.Plugin;

namespace Microsoft.PowerToys.Run.Plugin.PowerToysCommands
{
    // This class manages all aspects of results for the PowerToys
    // Commands plugin. It creates the list of all possible commands
    // as Results and allows callers to query for the matching subset
    // and handles invoking the commands. Invocation of commands is
    // via setting named events which the different PowerToys
    // commands are waiting on.
    public class CommandResults
    {
        // Enum of the different PowerToys Commands
        // sorted alphabetically.
        public enum Command
        {
            ColorPicker,
            FancyZonesEditor,
            MeasureTool,
            PowerOCR,
            ShortcutGuide,
        }

        public CommandResults()
        {
            // The different PowerToys Commands Results
            // sorted alphabetically. This should be one to
            // one with the Command enum and Result indicies should
            // match their corresponding enum value.
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
            ArgumentNullException.ThrowIfNull(query, nameof(query));

            IEnumerable<Result> results = Enumerable.Empty<Result>();

            // If provided no search part of the query, which can happen
            // when invoking the plugin directly but without yet entering
            // a search query, then return no commands.
            if (!string.IsNullOrEmpty(query.Search))
            {
                results = AllCommandResults.Where(
                    commandResult => DoesResultMatchQuery(commandResult, query));
            }

            return results;
        }

        // Return true if the result's title or subtitle partially matches the queries search
        // ignoring case.
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
            bool actionWasApplied;

            switch (command)
            {
                case Command.ColorPicker: actionWasApplied = SetEvent(Constants.ShowColorPickerSharedEvent()); break;
                case Command.FancyZonesEditor: actionWasApplied = SetEvent(Constants.FZEToggleEvent()); break;
                case Command.MeasureTool: actionWasApplied = SetEvent(Constants.MeasureToolTriggerEvent()); break;
                case Command.PowerOCR: actionWasApplied = SetEvent(Constants.ShowPowerOCRSharedEvent()); break;
                case Command.ShortcutGuide: actionWasApplied = SetEvent(Constants.ShortcutGuideTriggerEvent()); break;
                default: throw new NotImplementedException(
                    "Microsoft.PowerToys.Run.Plugin.PowerToysCommands.CommandResults command enum not implemented: "
                    + command);
            }

            return actionWasApplied;
        }
    }
}
