using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildCs.FileSystem;

namespace BuildCs.Zip
{
    public class ZipArgs
    {
        private readonly List<ZipBuildItem> _items;

        public ZipArgs()
        {
            _items = new List<ZipBuildItem>();
        }

        public IReadOnlyList<ZipBuildItem> Items
        {
            get { return _items; }
        }

        public void AddItem(BuildItem path, BuildItem destinationDir = null)
        {
            _items.Add(new ZipBuildItem(path, destinationDir));
        }

        public void AddItems(IEnumerable<BuildItem> items, BuildItem destinationDir = null)
        {
            items.Each(i => _items.Add(new ZipBuildItem(i, destinationDir)));
        }
    }
}