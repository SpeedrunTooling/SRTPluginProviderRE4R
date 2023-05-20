using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SRTPluginProviderRE4R.Structs
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x38)]
    public struct DictionaryStruct
    {
        [FieldOffset(0x18)] private nint _entries;
        [FieldOffset(0x20)] private int _count;

        public IntPtr Entries => IntPtr.Add(_entries, 0x0);
        public int Count => _count;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x38)]
    public struct ListStruct
    {
        [FieldOffset(0x10)] private nint _items;
        [FieldOffset(0x18)] private int _size;

        public IntPtr Items => IntPtr.Add(_items, 0x0);
        public int Count => _size;
    }
}
