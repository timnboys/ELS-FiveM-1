﻿/*
    ELS FiveM - A ELS implementation for FiveM
    Copyright (C) 2017  E.J. Bevenour

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELS.configuration
{
    internal delegate void ControlsUpdatedhandler(configuration.ControlConfiguration.ELSControls elsControls);
    class ControlConfiguration
    {

        public static event ControlsUpdatedhandler ControlsUpdated;
        public static ELSControls KeyBindings = new ELSControls();
        internal ControlConfiguration()
        {
            FileLoader.OnSettingsLoaded += FileLoader_OnSettingsLoaded;
        }

        private void FileLoader_OnSettingsLoaded(SettingsType.Type type, string Data)
        {
            if (type == SettingsType.Type.GLOBAL)
            {
                var u = SharpConfig.Configuration.LoadFromString(Data);
                var t = u["CONTROL"]["Sound_Ahorn"].IntValue;
                KeyBindings.Sound_Ahorn = (Control)t;

                t = u["CONTROL"]["Snd_SrnTon1"].IntValue;
                KeyBindings.Snd_SrnTon1 = (Control)t;

                t = u["CONTROL"]["Snd_SrnTon2"].IntValue;
                KeyBindings.Snd_SrnTon2 = (Control)t;

                t = u["CONTROL"]["Snd_SrnTon3"].IntValue;
                KeyBindings.Snd_SrnTon3 = (Control)t;

                t = u["CONTROL"]["Snd_SrnTon4"].IntValue;
                KeyBindings.Snd_SrnTon4 = (Control)t;

                t = u["CONTROL"]["Sound_Manul"].IntValue;
                KeyBindings.Sound_Manul = (Control)t;

                t = u["CONTROL"]["Toggle_SIRN"].IntValue;
                KeyBindings.Toggle_SIRN = (Control)t;

                t = u["CONTROL"]["Toggle_DSRN"].IntValue;
                KeyBindings.Toggle_DSRN = (Control)t;

                t = u["CONTROL"]["TogInfoPanl"].IntValue;
                KeyBindings.TogInfoPanl = (Control)t;

                t = u["CONTROL"]["Snd_SrnPnic"].IntValue;
                KeyBindings.Snd_SrnPnic = (Control)t;

                ControlsUpdated?.Invoke(KeyBindings);

            }
        }
        internal class ELSControls
        {
            internal Control Toggle_SIRN { get; set; }
            internal Control Sound_Ahorn { get; set; }
            internal Control Snd_SrnTon1 { get; set; }
            internal Control Snd_SrnTon2 { get; set; }
            internal Control Snd_SrnTon3 { get; set; }
            internal Control Snd_SrnTon4 { get; set; }
            internal Control Snd_SrnPnic { get; set; }
            internal Control Sound_Manul { get; set; }
            internal Control Toggle_DSRN { get; set; }
            internal Control TogInfoPanl { get; set; }
        }

        internal static bool isValidData(string data)
        {
            return SharpConfig.Configuration.LoadFromString(data).Contains("CONTROL", "Toggle_WRNL");
        }
    }
}