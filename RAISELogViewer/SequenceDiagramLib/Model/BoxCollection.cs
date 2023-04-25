using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SequenceDiagramLib.Model
{
	public class BoxCollection : IEnumerable<Box>
	{
		private Sequence sequence = null;

		private List<Box> boxes = new List<Box>();
		private Dictionary<string, Box> boxesDict = new Dictionary<string, Box>();

		public BoxCollection(Sequence sequence)
		{
			this.sequence = sequence;
		}

		public int Count
		{
			get
			{
				return this.boxes.Count;
			}
		}

		public Box this[int index]
		{
			get
			{
				return this.boxes[index];
			}
		}

		public Box this[string key]
		{
			get
			{
				return this.boxesDict[key];
			}
		}

		public Box Create(BoxInfo boxInfo)
		{
			Box box = new Box(this.sequence, boxInfo, this.boxes.Count);
			this.boxes.Add(box);
			this.boxesDict.Add(boxInfo.Name, box);

			return box;
		}

		public Box Create(string name, Color? color = null)
		{
			BoxInfo boxInfo = new BoxInfo();
			boxInfo.Name = name;
			return Create(boxInfo);
		}

		public Box CreateOrGet(BoxInfo boxInfo)
		{
			if (!this.boxesDict.ContainsKey(boxInfo.Name))
				Create(boxInfo);

			return this.boxesDict[boxInfo.Name];
		}

		public Box CreateOrGet(string name, Color? color = null)
		{
			if (!this.boxesDict.ContainsKey(name))
				Create(name, color);

			return this.boxesDict[name];
		}
		#region

		public IEnumerator<Box> GetEnumerator()
		{
			return this.boxes.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
	}
}
