using odm.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using onvif.services;
using video.player;
using onvif.utils;
using utils;
using System.Net;
using FireDeptStopwatch.Classes;

namespace FireDeptStopwatch.Helpers
{
    public static class CameraHelper
    {
        public static string GetStreamUri(CameraInfo cameraInfo)
        {
            var cameraUrl = BuildCameraUrl(cameraInfo.Url);

            NvtSessionFactory factory = new NvtSessionFactory(new NetworkCredential(cameraInfo.User, cameraInfo.Password));

            var session = factory
                .CreateSession(new Uri(cameraUrl));

            var profiles = session
                .GetProfiles()
                .RunSynchronously();

            if (profiles == null || profiles.Length == 0)
            {
                return null;
            }

            var streamSetup = new StreamSetup()
            {
                stream = StreamType.rtpUnicast,
                transport = new Transport()
                {
                    protocol = TransportProtocol.udp
                }
            };

            var profile = profiles.First();

            var streamUri = session
                .GetStreamUri(streamSetup, profile.token)
                .RunSynchronously();

            if (streamUri == null)
            {
                return null;
            }

            return streamUri.ToString();
        }

        public static string BuildCameraUrl(string cameraUrl)
        {
            if (!cameraUrl.StartsWith("http://"))
            {
                cameraUrl = "http://" + cameraUrl;
            }
            cameraUrl += "/onvif/device_service";

            return cameraUrl;
        }
    }
}
