﻿using System;
using System.Collections.Generic;
using CitizenFX.Core;
using ELS.configuration;

namespace ELS
{
    public class ELSVehicle : PoolObject, FullSync.IFullSyncComponent
    {
        private Siren.Siren _siren;
        private Vehicle _vehicle;
        private VCF.vcfroot _vcf;
        public ELSVehicle(int handle, IDictionary<string, object> data) : base(handle)
        {
            _vehicle = new Vehicle(handle);
            ModelLoaded();

            if (_vehicle.DisplayName == "CARNOTFOUND" || _vehicle.GetNetworkId()==0) {
                throw new Exception("Vehicle creation failure.");
            }
            else if( VCF.ELSVehicle.Exists(item => item.Item2.FileName == _vehicle.DisplayName))
            {
                _vcf = VCF.ELSVehicle.Find(item => item.Item2.FileName == _vehicle.DisplayName).Item2;
            }
            if (data.ContainsKey("Siren"))
            {
                _siren = new Siren.Siren(_vehicle, _vcf,(IDictionary<string, object>)data["Siren"]);

            }
            else
            {
                //_vehicle.SetExistOnAllMachines(true);
#if DEBUG
                CitizenFX.Core.Debug.WriteLine(CitizenFX.Core.Native.API.IsEntityAMissionEntity(_vehicle.Handle).ToString());

                CitizenFX.Core.Debug.WriteLine($"registering netid:{_vehicle.GetNetworkId()}\n" +
                    $"Does entity belong to this script:{CitizenFX.Core.Native.API.DoesEntityBelongToThisScript(_vehicle.Handle, false)}");

#endif
                _siren = new Siren.Siren(_vehicle,_vcf);
            }
#if DEBUG
            CitizenFX.Core.Debug.WriteLine($"created vehicle");
#endif
        }
         private async void  ModelLoaded()
        {
            while (_vehicle.DisplayName == "CARNOTFOUND")
            {
                await CitizenFX.Core.BaseScript.Delay(0);
            }
        }
        internal void CleanUP()
        {
            _siren.CleanUP();
            CitizenFX.Core.Debug.WriteLine("running vehicle deconstructor");
            CitizenFX.Core.Native.API.NetworkUnregisterNetworkedEntity(_vehicle.Handle);
            //CitizenFX.Core.Native.API.NetworkSetMissionFinished();
            //_vehicle.MarkAsNoLongerNeeded();
        }

        internal void RunTick()
        {
            _siren.Ticker();
        }
        internal void RunExternalTick()
        {
            _siren.ExternalTicker();
        }
        internal Vector3 GetBonePosistion()
        {
            return _vehicle.Bones["door_dside_f"].Position;
        }
        public override bool Exists()
        {
            return CitizenFX.Core.Native.Function.Call<bool>(CitizenFX.Core.Native.Hash.DOES_ENTITY_EXIST, _vehicle);
        }

        public override void Delete()
        {
            _vehicle.Delete();
        }
        /// <summary>
        /// Proxies sync data to te lighting and siren sub components
        /// </summary>
        /// <param name="dataDic"></param>
        internal void SetSyncDataSets(IDictionary<string, object> dataDic)
        {
            var sirenDic = dataDic["siren"];
            _siren.SetData(dataDic);
        }
        internal void UpdateRemoteSiren(string command, bool state)
        {
            _siren.SirenControlsRemote(command, state);
        }
        internal int GetNetworkId()
        {
            return _vehicle.GetNetworkId();
        }

        public void SetData(IDictionary<string, object> data)
        {
            _siren.SetData((IDictionary<string, object>)data["siren"]);
        }

        public Dictionary<string, object> GetData()
        {
            Dictionary<string, object> vehDic = new Dictionary<string, object>
            {
                {"siren",_siren.GetData() },
                {"Lights",null },
                {"NetworkID",_vehicle.GetNetworkId() }
            };
            return vehDic;
        }
    }
}