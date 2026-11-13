using System.Collections.Generic;
using UnityEngine;
using UnityModManagerNet;

namespace SolastaZoomMod
{
    public static class Main
    {
        // Wider Camera sets the top orbit radius to 24. This raises that limit to 40.
        private const float TopHeight = 42.9f;
        private const float TopRadius = 40f;
        private const float MiddleHeight = 21.4f;
        private const float MiddleRadius = 20f;
        private const float BottomHeight = 5.36f;
        private const float BottomRadius = 5f;

        private static float nextCheck;
        private static bool hasLoggedChange;
        private static readonly HashSet<int> adjustedCameraIds = new HashSet<int>();
        private static readonly HashSet<int> adjustedParameterIds = new HashSet<int>();

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnUpdate = OnUpdate;
            modEntry.Logger.Log("Solasta Zoom Mod loaded. Max orbit radius: " + TopRadius);
            return true;
        }

        private static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            if (Time.unscaledTime < nextCheck)
            {
                return;
            }

            nextCheck = Time.unscaledTime + 0.5f;

            // Only adjust the cameras while a game location is active.
            if (ServiceRepository.GetService<IGameLocationService>() == null)
            {
                return;
            }

            CameraModeManual[] cameraModes = Object.FindObjectsOfType<CameraModeManual>();
            foreach (CameraModeManual cameraMode in cameraModes)
            {
                if (cameraMode == null || cameraMode.Parameters == null)
                {
                    continue;
                }

                float[] defaultRadii = cameraMode.Parameters.DefaultOrbitsRadii;
                if (defaultRadii == null || defaultRadii.Length < 3)
                {
                    continue;
                }

                // Solasta's AdjustOrbit routine restores these defaults during camera
                // updates. Updating the defaults lets the game keep the wider range
                // without repeatedly changing the live camera and causing flicker.
                int parameterId = cameraMode.Parameters.GetInstanceID();
                if (adjustedParameterIds.Add(parameterId))
                {
                    defaultRadii[0] = TopRadius;
                    defaultRadii[1] = MiddleRadius;
                    defaultRadii[2] = BottomRadius;
                }

                var camera = cameraMode.FreeLookCamera;
                if (camera == null || camera.m_Orbits == null || camera.m_Orbits.Length < 3 ||
                    !adjustedCameraIds.Add(camera.GetInstanceID()))
                {
                    continue;
                }

                camera.m_Orbits[0].m_Height = TopHeight;
                camera.m_Orbits[0].m_Radius = TopRadius;
                camera.m_Orbits[1].m_Height = MiddleHeight;
                camera.m_Orbits[1].m_Radius = MiddleRadius;
                camera.m_Orbits[2].m_Height = BottomHeight;
                camera.m_Orbits[2].m_Radius = BottomRadius;

                if (!hasLoggedChange)
                {
                    modEntry.Logger.Log("Camera defaults increased. Far orbit radius: " + TopRadius);
                    hasLoggedChange = true;
                }
            }
        }
    }
}
