using System;
using System.Threading;

namespace Serilog.Enrichers.Sensitive
{
    public class SensitiveArea : IDisposable
    {
        private static readonly AsyncLocal<SensitiveArea> InstanceLocal = new AsyncLocal<SensitiveArea>();

        internal readonly MaskingItem[] _maskingItems;

        public SensitiveArea(params MaskingItem[] maskingItems)
        {
            if (maskingItems == null || maskingItems.Length == 0)
            {
                _maskingItems = new MaskingItem[0];
            }
            else
            {
                _maskingItems = maskingItems;
            }
        }

        public static SensitiveArea Instance
        {
            get => InstanceLocal.Value;
            set => InstanceLocal.Value = value;
        }

        public static MaskingItem[] InstanceMaskingRules
        {
            get => InstanceLocal.Value._maskingItems;

        }

        public void Dispose()
        {
            Instance = null;
        }
    }
}