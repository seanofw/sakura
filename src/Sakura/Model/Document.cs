namespace Sakura.Model
{
	public class Document
	{
		public int RootIndex { get; set; }
		public Group Root => History[RootIndex];
		public string Name { get; set; }

		public List<Group> History { get; } = new List<Group> { new Group() };

		public void Do(Group group)
		{
			// Remove the undone work, if it exists.
			if (RootIndex + 1 < History.Count)
				History.RemoveRange(RootIndex + 1, History.Count - (RootIndex + 1));

			History.Add(group);
			RootIndex++;
		}

		public void Undo()
		{
			if (RootIndex > 0) RootIndex--;
		}

		public void Redo()
		{
			if (RootIndex < History.Count - 1) RootIndex++;
		}
	}
}
