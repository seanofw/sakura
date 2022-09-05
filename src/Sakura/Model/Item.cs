using System.Threading;

namespace Sakura.Model
{
	public abstract class Item
	{
		public long Id { get; }
		public Bounds Bounds { get; }

		public Vector2 Center => (Bounds.Min + Bounds.Max) * 0.5f;

		private static long _idSource = 1;

		public ItemMetadata? Metadata { get; }

		protected Item(Bounds bounds, ItemMetadata? metadata = null, long id = -1)
		{
			Id = id > 0 ? id : Interlocked.Increment(ref _idSource);
			Bounds = bounds;
			Metadata = metadata;
		}

		public abstract Item WithMetadata(ItemMetadata? metadata);
		public abstract Item Clone();
		public abstract Item CloneWithNewId();
	}
}
