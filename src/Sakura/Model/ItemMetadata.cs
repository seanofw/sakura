namespace Sakura.Model
{
	public class ItemMetadata
	{
		public string? Name { get; }
		public Matrix3x2 Transform { get; }

		public ItemMetadata(string? name, Matrix3x2? transform = null)
		{
			Name = name;
			Transform = transform ?? Matrix3x2.Identity;
		}

		public ItemMetadata WithName(string name)
			=> new ItemMetadata(name, Transform);
		public ItemMetadata WithMatrix(Matrix3x2 transform)
			=> new ItemMetadata(Name, transform);

		public ItemMetadata Rotate(float angleInRadians)
			=> new ItemMetadata(Name, Transform * Matrix3x2.CreateRotation(angleInRadians));
		public ItemMetadata Scale(float x, float y)
			=> new ItemMetadata(Name, Transform * Matrix3x2.CreateScale(x, y));
		public ItemMetadata Translate(float x, float y)
			=> new ItemMetadata(Name, Transform * Matrix3x2.CreateTranslation(x, y));
	}
}