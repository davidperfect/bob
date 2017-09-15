using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderBookLib.Synchonization
{
    class VersionLock
    {
        int _awaitedVersion;
        Task _versionReached = new Task(() => { });

        public VersionLock(
             int awaitedVersion)
        {
            _awaitedVersion = awaitedVersion;
        }

        public void OnVersionUpdated(object sender, int version)
        {
            if (version >= _awaitedVersion)
            {
                _versionReached.Start();
            }
        }

        public async Task WaitForVersionAsync()
        {
            await _versionReached;
        }
    }
}
