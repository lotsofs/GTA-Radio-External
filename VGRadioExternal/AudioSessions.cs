using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace VGRadioExternal {
    /// <summary>
    /// Mutes apps through Core Audio, at the per-app level that the Windows Volume Mixer shows
    /// </summary>
    static class AudioSessions {

        const int DataFlowRender = 0;
        const int DeviceStateActive = 1;
        const int ClsCtxAll = 23;

        [ComImport, Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
        class MMDeviceEnumerator { }

        // These declare methods in vtable order, and stop at the last one that's needed

        [ComImport, Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IMMDeviceEnumerator {
            void EnumAudioEndpoints(int dataFlow, int stateMask, out IMMDeviceCollection devices);
        }

        [ComImport, Guid("0BD7A1BE-7A1A-44DB-8397-CC5392387B5E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IMMDeviceCollection {
            void GetCount(out int count);
            void Item(int index, out IMMDevice device);
        }

        [ComImport, Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IMMDevice {
            void Activate(ref Guid iid, int clsCtx, IntPtr activationParams, [MarshalAs(UnmanagedType.IUnknown)] out object manager);
        }

        [ComImport, Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IAudioSessionManager2 {
            void GetAudioSessionControl(IntPtr sessionGuid, int flags, out IntPtr sessionControl);
            void GetSimpleAudioVolume(IntPtr sessionGuid, int flags, out IntPtr audioVolume);
            void GetSessionEnumerator(out IAudioSessionEnumerator sessions);
        }

        [ComImport, Guid("E2F5BB11-0570-40CA-ACDD-3AA01277DEE8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IAudioSessionEnumerator {
            void GetCount(out int count);
            void GetSession(int index, [MarshalAs(UnmanagedType.IUnknown)] out object session);
        }

        [ComImport, Guid("BFB7FF88-7239-4FC9-8FA2-07C950BE9C6D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IAudioSessionControl2 {
            void GetState(out int state);
            void GetDisplayName(out IntPtr name);
            void SetDisplayName(IntPtr name, IntPtr context);
            void GetIconPath(out IntPtr path);
            void SetIconPath(IntPtr path, IntPtr context);
            void GetGroupingParam(out Guid groupingParam);
            void SetGroupingParam(IntPtr groupingParam, IntPtr context);
            void RegisterAudioSessionNotification(IntPtr notification);
            void UnregisterAudioSessionNotification(IntPtr notification);
            void GetSessionIdentifier(out IntPtr identifier);
            void GetSessionInstanceIdentifier(out IntPtr identifier);
            void GetProcessId(out int processId);
        }

        [ComImport, Guid("87CE5498-68D6-44E5-9215-6DA47EF883D8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface ISimpleAudioVolume {
            void SetMasterVolume(float level, IntPtr context);
            void GetMasterVolume(out float level);
            void SetMute([MarshalAs(UnmanagedType.Bool)] bool mute, IntPtr context);
        }

        /// <summary>
        /// Mutes or unmutes every audio session of every process with this name, on every output device.
        /// </summary>
        /// <param name="processName">the name of the process, without .exe</param>
        /// <param name="mute">whether to mute or unmute</param>
        /// <returns>how many sessions it found</returns>
        public static int SetMute(string processName, bool mute) {
            // an app can have many processes, and any one of them can own its session
            HashSet<int> processIds = new HashSet<int>(Process.GetProcessesByName(processName).Select(p => p.Id));

            int found = 0;
            IMMDeviceEnumerator enumerator = (IMMDeviceEnumerator)new MMDeviceEnumerator();
            IMMDeviceCollection devices = null;
            try {
                enumerator.EnumAudioEndpoints(DataFlowRender, DeviceStateActive, out devices);
                devices.GetCount(out int deviceCount);
                for (int i = 0; i < deviceCount; i++) {
                    devices.Item(i, out IMMDevice device);
                    try {
                        SetMute(device, processIds, mute, ref found);
                    }
                    catch (COMException) {
                        // some devices refuse to list their sessions, which shouldn't stop the others
                    }
                    finally {
                        Marshal.ReleaseComObject(device);
                    }
                }
            }
            finally {
                Release(devices);
                Release(enumerator);
            }
            return found;
        }

        static void SetMute(IMMDevice device, HashSet<int> processIds, bool mute, ref int found) {
            object manager = null;
            IAudioSessionEnumerator sessions = null;
            try {
                Guid managerId = typeof(IAudioSessionManager2).GUID;
                device.Activate(ref managerId, ClsCtxAll, IntPtr.Zero, out manager);
                ((IAudioSessionManager2)manager).GetSessionEnumerator(out sessions);
                sessions.GetCount(out int sessionCount);
                for (int i = 0; i < sessionCount; i++) {
                    sessions.GetSession(i, out object session);
                    try {
                        ((IAudioSessionControl2)session).GetProcessId(out int processId);
                        if (processIds.Contains(processId)) {
                            ((ISimpleAudioVolume)session).SetMute(mute, IntPtr.Zero);
                            found++;
                        }
                    }
                    finally {
                        Marshal.ReleaseComObject(session);
                    }
                }
            }
            finally {
                Release(sessions);
                Release(manager);
            }
        }

        static void Release(object comObject) {
            if (comObject != null) {
                Marshal.ReleaseComObject(comObject);
            }
        }
    }
}
