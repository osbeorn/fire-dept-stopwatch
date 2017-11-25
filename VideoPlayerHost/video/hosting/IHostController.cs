using System;

namespace video.hosting
{
    public interface IHostController
    {
        void Bye();
        Action<IHostController> Hello();
        bool isAlive();
    }
}

