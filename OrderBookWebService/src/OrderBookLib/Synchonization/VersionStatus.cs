using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderBookLib.Synchonization
{
    class VersionStatus
    {
        private int _version = 0;
        private event EventHandler<int> OnVersionUpdated;
        private Object _lock = new Object();

        public void Update(int version)
        {
            _version = version;
            OnVersionUpdated?.Invoke(this, version);
        }

        public async Task WaitForVersionAsync(int awaitedVersion)
        {
            VersionLock versionLock;
            lock (_lock)
            {
                if (_version >= awaitedVersion)
                {
                    return;
                }

                versionLock = new VersionLock(awaitedVersion);
                OnVersionUpdated += versionLock.OnVersionUpdated;
            }

            await versionLock.WaitForVersionAsync();
            OnVersionUpdated -= versionLock.OnVersionUpdated;
        }
    }
}
