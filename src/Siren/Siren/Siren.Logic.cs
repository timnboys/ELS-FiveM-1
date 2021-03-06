using CitizenFX.Core;
using CitizenFX.Core.UI;
using ELS.configuration;
using Control = CitizenFX.Core.Control;

namespace ELS.Siren
{
    partial class Siren : IManagerEntry
    {
        void AirHornLogic(bool pressed, bool disableControls = false)
        {
            if (disableControls)
            {
                Game.DisableControlThisFrame(0, configuration.ControlConfiguration.KeyBindings.Sound_Ahorn);
                Game.DisableControlThisFrame(2, Control.ScriptPadDown);
            }
            if (pressed)
            {

                if (_vcf.SOUNDS.MainHorn.InterruptsSiren)
                {
                    if (_mainSiren._enable)
                    {
                        _mainSiren.interupted = true;
                        _mainSiren.SetEnable(false);
                    }
                }
                _tones.horn.SetState(true);
            }
            if (!pressed)
            {
                _tones.horn.SetState(false);

                if (_vcf.SOUNDS.MainHorn.InterruptsSiren &&
                    _mainSiren.interupted)
                {
                    _mainSiren.interupted = false;
                    _mainSiren.SetEnable(true);
                }
            }
        }

        void ManualTone1Logic(bool pressed, bool disableControls = false)
        {
            if (disableControls) Game.DisableControlThisFrame(0, ControlConfiguration.KeyBindings.Snd_SrnTon1);

            if (pressed)
            {
                if (_mainSiren._enable) //Is the MainSiren Active
                {
                    _mainSiren.setMainTone(_tones.tone1);
                }
                else
                {
                    _tones.tone1.SetState(!_tones.tone1._state);
                }
            }

        }
        void ManualTone2Logic(bool pressed, bool disableControls = false)
        {
            if (disableControls) Game.DisableControlThisFrame(0, ControlConfiguration.KeyBindings.Snd_SrnTon2);

            if (pressed)
            {

                if (_mainSiren._enable)
                {
                    _mainSiren.setMainTone(_tones.tone2);
                }
                else
                {
                    _tones.tone2.SetState(!_tones.tone2._state);
                }
            }
        }
        void ManualTone3Logic(bool pressed, bool disableControls = false)
        {
            if (disableControls) Game.DisableControlThisFrame(0, ControlConfiguration.KeyBindings.Snd_SrnTon3);
            if (pressed)
            {
                if (_mainSiren._enable)
                {
                    _mainSiren.setMainTone(_tones.tone3);
                }
                else
                {
                    _tones.tone3.SetState(!_tones.tone3._state);
                }
            }
        }
        void ManualTone4Logic(bool pressed, bool disableControls = false)
        {
            if (disableControls) Game.DisableControlThisFrame(0, ControlConfiguration.KeyBindings.Snd_SrnTon4);
            if (pressed)
            {
                if (_mainSiren._enable)
                {
                    _mainSiren.setMainTone(_tones.tone4);
                }
                else
                {
                    _tones.tone4.SetState(!_tones.tone4._state);
                }
            }
        }

        private void MainSirenToggleLogic(bool toggle, bool disableControls = false)
        {
            if (disableControls) Game.DisableControlThisFrame(0, ControlConfiguration.KeyBindings.Toggle_SIRN);
            if (toggle)
            {
                _mainSiren.SetEnable(!_mainSiren._enable);
            }
        }

        void ManualSoundLogic(bool pressed, bool disableControls = false)
        {
            if (disableControls) Game.DisableControlThisFrame(0, ControlConfiguration.KeyBindings.Sound_Manul);
            if (pressed)
            {
                if (!_mainSiren._enable || (!_mainSiren._enable && _vcf.SOUNDS.MainHorn.InterruptsSiren &&
                                           _tones.horn._state))
                {
                    _tones.tone1.SetState(true);
                }
                else
                {
                    _mainSiren.nextTone();
                }
            }
            else
            {
                if (!_mainSiren._enable || (!_mainSiren._enable && _vcf.SOUNDS.MainHorn.InterruptsSiren &&
                                           _tones.horn._state))
                {
                    _tones.tone1.SetState(false);
                }
                else
                {
                    _mainSiren.previousTone();
                }
            }
        }

        void DualSirenLogic(bool toggle, bool disableControls = false)
        {
            if (disableControls) Game.DisableControlThisFrame(0, ControlConfiguration.KeyBindings.Toggle_DSRN);
            if (toggle)
            {
                dual_siren = !dual_siren;
                Screen.ShowNotification($"Dual Siren {dual_siren}");
            }
        }
        void PanicAlarmLogic(bool toggle,bool disableControls = false)
        {
            if (disableControls) Game.DisableControlThisFrame(0, ControlConfiguration.KeyBindings.Snd_SrnPnic);
            CitizenFX.Core.UI.Screen.ShowNotification($"{toggle}");
            this._tones.panicAlarm.SetState(toggle);
            if (toggle)
            {
                //Screen.ShowNotification($"{toggle}");
            }
        }
    }
}