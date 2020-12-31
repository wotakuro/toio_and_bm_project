//
// Copyright (c) 2020-present, Sony Interactive Entertainment Inc.
//
// This source code is licensed under the MIT license found in the
// LICENSE file in the root directory of this source tree.
//
#if UNITY_ANDROID && !UNITY_EDITOR
#define UNITY_ANDROID_RUNTIME
#endif

using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace toio
{
    public class Ble
    {


        public static void Initialize(Action initializedAction, Action<string> errorAction = null)
        {
#if UNITY_IOS
            toio.BleiOS.Initialize(initializedAction, errorAction);
#elif UNITY_ANDROID_RUNTIME
            toio.Android.BleAndroid.Initialize(initializedAction, errorAction);
#endif
        }

        public static void Finalize(Action finalizedAction = null)
        {
#if UNITY_IOS
            toio.BleiOS.Finalize(finalizedAction);
#elif UNITY_ANDROID_RUNTIME
            toio.Android.BleAndroid.Finalize(finalizedAction);
#endif
        }

        public static void EnableBluetooth(bool enable)
        {
#if UNITY_IOS
            toio.BleiOS.EnableBluetooth(enable);
#elif UNITY_ANDROID_RUNTIME
            toio.Android.BleAndroid.EnableBluetooth(enable);
#endif
        }

        public static void StartScan(string[] serviceUUIDs, Action<string, string, int, byte[]> discoveredAction = null)
        {
#if UNITY_IOS
            toio.BleiOS.StartScan(serviceUUIDs, discoveredAction);
#elif UNITY_ANDROID_RUNTIME
            toio.Android.BleAndroid.StartScan(serviceUUIDs, discoveredAction);
#endif
        }

        public static void StopScan()
        {
#if UNITY_IOS
            toio.BleiOS.StopScan();
#elif UNITY_ANDROID_RUNTIME
            toio.Android.BleAndroid.StopScan();
#endif
        }

        public static void ConnectToPeripheral(string identifier, Action<string> connectedPeripheralAction = null, Action<string, string> discoveredServiceAction = null, Action<string, string, string> discoveredCharacteristicAction = null, Action<string> disconnectedPeripheralAction = null)
        {
#if UNITY_IOS
            toio.BleiOS.ConnectToPeripheral(identifier, connectedPeripheralAction, discoveredServiceAction,
                discoveredCharacteristicAction, disconnectedPeripheralAction);
#elif UNITY_ANDROID_RUNTIME
            toio.Android.BleAndroid.ConnectToPeripheral(identifier,connectedPeripheralAction,
                discoveredServiceAction,discoveredCharacteristicAction,disconnectedPeripheralAction);
#endif
        }

        public static void DisconnectPeripheral(string identifier, Action<string> disconnectedPeripheralAction = null)
        {
#if UNITY_IOS
            toio.BleiOS.DisconnectPeripheral(identifier, disconnectedPeripheralAction);
#elif UNITY_ANDROID_RUNTIME
            toio.Android.BleAndroid.DisconnectPeripheral(identifier, disconnectedPeripheralAction);
#endif
        }

        public static void DisconnectAllPeripherals()
        {
#if UNITY_IOS
            toio.BleiOS.DisconnectAllPeripherals();
#elif UNITY_ANDROID_RUNTIME
            toio.Android.BleAndroid.DisconnectAllPeripherals();
#endif
        }

        public static void ReadCharacteristic(string identifier, string serviceUUID, string characteristicUUID, Action<string, string, byte[]> didReadChracteristicAction)
        {
#if UNITY_IOS
            toio.BleiOS.ReadCharacteristic(identifier, serviceUUID,
                characteristicUUID, didReadChracteristicAction);
#elif UNITY_ANDROID_RUNTIME
            toio.Android.BleAndroid.ReadCharacteristic(identifier, serviceUUID,characteristicUUID,didReadChracteristicAction);
#endif
        }

        public static void WriteCharacteristic(string identifier, string serviceUUID, string characteristicUUID, byte[] data, int length, bool withResponse, Action<string, string> didWriteCharacteristicAction)
        {
#if UNITY_IOS
            toio.BleiOS.WriteCharacteristic(identifier, serviceUUID, characteristicUUID,
                data, length, withResponse, didWriteCharacteristicAction);
#elif UNITY_ANDROID_RUNTIME
            toio.Android.BleAndroid.WriteCharacteristic(identifier, serviceUUID, characteristicUUID, data,length,withResponse,didWriteCharacteristicAction);
#endif
        }

        public static void SubscribeCharacteristic(string identifier, string serviceUUID, string characteristicUUID, Action<string, string, byte[]> notifiedCharacteristicAction)
        {
#if UNITY_IOS
            toio.BleiOS.SubscribeCharacteristic(identifier, serviceUUID,
                characteristicUUID, notifiedCharacteristicAction);
#elif UNITY_ANDROID_RUNTIME
            toio.Android.BleAndroid.SubscribeCharacteristic(identifier, serviceUUID, characteristicUUID, notifiedCharacteristicAction);
#endif
        }

        public static void UnSubscribeCharacteristic(string identifier, string serviceUUID, string characteristicUUID, Action<string> action)
        {
#if UNITY_IOS
            toio.BleiOS.UnSubscribeCharacteristic(identifier, serviceUUID, characteristicUUID, action);
#elif UNITY_ANDROID_RUNTIME
            toio.Android.BleAndroid.UnSubscribeCharacteristic(identifier, serviceUUID, characteristicUUID, action);
#endif
        }
    }
}
