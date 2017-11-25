using System;

namespace video.player
{
    public interface IPlayer : IDisposable
    {
        IDisposable Play(MediaStreamInfo mediaStreamInfo, IPlaybackController playbackController);
        void SetMetadataReciever(IMetadataReceiver metadataReceiver);
        void SetVideoBuffer(VideoBuffer videoBuffer);
    }
}

