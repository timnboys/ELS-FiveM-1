﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using System.Drawing;
using System.Collections;

namespace ELS.Manager
{
    class VehicleManager : Manager
    {
        public VehicleManager() : base()
        {
        }

        internal override void RunTick()
        {
            ELSVehicle _currentVehicle;
            if (Game.PlayerPed.IsInVehicle()
                && Game.PlayerPed.IsSittingInVehicle()
                && Game.PlayerPed.CurrentVehicle.IsEls()
                && (
                    Game.PlayerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) == Game.PlayerPed
                    || Game.PlayerPed.CurrentVehicle.GetPedOnSeat(VehicleSeat.Passenger) == Game.PlayerPed
                )
            )
            {
                //Screen.ShowNotification("adding vehicle");
                AddIfNotPresint(Game.PlayerPed.CurrentVehicle, vehicle: out _currentVehicle);
                _currentVehicle?.RunTick();
                if (Game.IsControlJustPressed(0, Control.Cover))
                {
                    FullSync.FullSyncManager.SendDataBroadcast(
                        _currentVehicle.GetData()
                    );
                    CitizenFX.Core.UI.Screen.ShowNotification("FullSync™ ran");
                    CitizenFX.Core.Debug.WriteLine("FullSync™ ran");
                }
            }
            foreach (ELSVehicle veh in Entities)
            {
                veh.RunExternalTick();
            }
#if DEBUG
            if (Game.Player.Character.LastVehicle == null) return;
            var bonePos = Game.Player.Character.LastVehicle.Bones["door_dside_f"].Position;
            var pos = Game.Player.Character.GetPositionOffset(bonePos);
            var text = new Text($"X:{pos.X} Y:{pos.Y} Z:{pos.Z} Lenght:{pos.Length()}", new PointF(Screen.Width / 2.0f, 10f), 0.5f);
            text.Alignment = Alignment.Center;
            if (pos.Length() < 1.5) text.Color = Color.FromArgb(255, 0, 0);
            text.Draw();
#endif

            //TODO Chnage how I check for the panic alarm
        }

        private bool AddIfNotPresint(Vehicle o, [Optional] IDictionary<string, object> data, [Optional]out ELSVehicle vehicle)
        {
            
            if (!Entities.Exists(poolObject => poolObject.Handle == o.Handle))
            {
                if (data == null) data = new Dictionary<string, object>();
                var veh = new ELSVehicle(o.Handle, data);
                Entities.Add(veh);
                vehicle = veh;
                return false;
            }
            else
            {
                vehicle = Entities.Find(poolObject => poolObject.Handle == o.Handle) as ELSVehicle;
                return true;
            }
        }
        //        internal void UpdateRemoteSirens(string command, int netId, int playerId, bool state)
        //        {
        //#if !REMOTETEST
        //            if (Game.Player.ServerId == playerId) return;
        //#endif
        //            var vehicle = new Vehicle(Function.Call<int>(Hash.NETWORK_GET_ENTITY_FROM_NETWORK_ID, netId));
        //            if (!CitizenFX.Core.Native.Function.Call<bool>(Hash.DOES_ENTITY_EXIST, vehicle))
        //            {
        //                Screen.ShowNotification("Vehicle does not exist");
        //                Screen.ShowNotification($"Net Status {Function.Call<bool>(Hash.NETWORK_GET_ENTITY_IS_NETWORKED, vehicle)}");
        //                return;
        //            };
        //            AddIfNotPresint(vehicle);
        //            ((ELSVehicle)Entities.Find(o => o.Handle == (int)vehicle.Handle)).UpdateRemoteSiren(command, state);


        //        }
        /// <summary>
        /// Proxies the sync data to a certain vehicle
        /// </summary>
        /// <param name="dataDic">data</param>
        async internal void SetVehicleSyncData(IDictionary<string, object> dataDic)
        {
            CitizenFX.Core.Debug.WriteLine("cerate evhc");
            var veh = new Vehicle(API.NetworkGetEntityFromNetworkId((int)dataDic["NetworkID"]));
            var bo = AddIfNotPresint(veh
                        , dataDic,out ELSVehicle veh1);
            veh1.SetData(dataDic);
        }

        internal static void SyncRequestReply(long NetworkId)
        {
            FullSync.FullSyncManager.SendDataBroadcast(
                ((ELSVehicle)Entities.Find(o => ((ELSVehicle)o).GetNetworkId() == NetworkId)).GetData()
            );
        }
        internal void SyncAllVehiclesOnFirstSpawn(System.Dynamic.ExpandoObject data)
        {
            dynamic k = data;
            var y = data.ToArray();
            foreach ( var struct1 in y)
            {
                int netID = int.Parse(struct1.Key);
                var vehData = (IDictionary<string,object>)struct1.Value;
                CitizenFX.Core.Debug.WriteLine($"{vehData["NetworkID"]}, {API.NetworkDoesEntityExistWithNetworkId(netID)}, {API.NetworkDoesNetworkIdExist(netID)}, {API.DoesEntityExist(API.NetworkGetEntityFromNetworkId(netID))}");
                AddIfNotPresint(new Vehicle(API.NetworkGetEntityFromNetworkId( (int)vehData["NetworkID"])),
                        vehData,
                        out ELSVehicle veh
                );
            }
        }

        void GetAllVehicles()
        {

        }
        internal void CleanUP()
        {
            foreach (var obj in Entities)
            {
                ((ELSVehicle)obj).CleanUP();
            }
        }
        ~VehicleManager()
        {
            CleanUP();
        }
    }
}