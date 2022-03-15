using System;
using System.Linq;
using Microsoft.Kinect;

namespace KinectApp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            KinectStart();
            Console.ReadKey();
        }

        private static void KinectStart()
        {
            var kinect = KinectSensor.KinectSensors.FirstOrDefault(s => s.Status == KinectStatus.Connected);
            if (null != kinect) {
                kinect.SkeletonStream.Enable();
                kinect.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(SkeletonFrameReady);
                kinect.Start();
            } else {
                Console.WriteLine("No connection.");
            }
        }

        static void SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs args)
        {
            using (var frame = args.OpenSkeletonFrame())
                if (frame != null) {
                    Skeleton[] skeletons = new Skeleton[frame.SkeletonArrayLength];
                    frame.CopySkeletonDataTo(skeletons);
                    if (skeletons.Length > 0) {
                        var user = skeletons.FirstOrDefault(
                            u => u.TrackingState == SkeletonTrackingState.Tracked
                        );

                        if (user != null) {
                            Console.WriteLine("User found.");
                        }
                    }
                }
        }
    }
}