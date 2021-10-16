using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Litdex.Security.RNG
{
	/// <summary>
	///		Partial class for Sequence.
	/// </summary>
	public abstract partial class Random : ISequence
	{
		/// <inheritdoc/>
		public virtual T Choice<T>(T[] items)
		{
			if (items.Length <= 0 || items == null)
			{
				throw new ArgumentNullException(nameof(items), "The items is empty or null.");
			}

			return items[(int)this.NextInt(0, (uint)(items.Length - 1))];
		}

		/// <inheritdoc/>
		public virtual T[] Choice<T>(T[] items, int select)
		{
			if (items.Length <= 0 || items == null)
			{
				throw new ArgumentNullException(nameof(items), "The items is empty or null.");
			}

			if (select < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(select), "The number of elements to be retrieved is negative or less than 1.");
			}
			else if (select > items.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(select), "The number of elements to be retrieved exceeds the items size.");
			}
			else if (select == items.Length)
			{
				return items;
			}

			var selected = new T[select];
			uint index;
			uint length = (uint)(items.Length - 1);

			for (var i = 0; i < select; i++)
			{
				index = this.NextInt(0, length);
				selected[i] = items[index];
			}

			return selected.ToArray();
		}

		/// <inheritdoc/>
		public virtual Task<T[]> ChoiceAsync<T>(T[] items, int select)
		{
			return this.ChoiceAsync(items, select, CancellationToken.None);
		}

		/// <inheritdoc/>
		public virtual Task<T[]> ChoiceAsync<T>(T[] items, int select, CancellationToken cancellationToken)
		{
			return Task.Factory.StartNew(() =>
			{
				return this.Choice(items, select);
			}, cancellationToken);
		}

		/// <inheritdoc/>
		public virtual T Choice<T>(IEnumerable<T> items)
		{
			return this.Choice(items.ToArray());
		}

		/// <inheritdoc/>
		public virtual T[] Choice<T>(IEnumerable<T> items, int select)
		{
			return this.Choice(items.ToArray(), select);
		}

		/// <inheritdoc/>
		public virtual Task<T[]> ChoiceAsync<T>(IEnumerable<T> items, int select)
		{
			return this.ChoiceAsync(items.ToArray(), select, CancellationToken.None);
		}

		/// <inheritdoc/>
		public virtual Task<T[]> ChoiceAsync<T>(IEnumerable<T> items, int select, CancellationToken cancellationToken)
		{
			return this.ChoiceAsync(items.ToArray(), select, cancellationToken);
		}

		/// <inheritdoc/>
		public virtual T[] Sample<T>(T[] items, int select)
		{
			if (items.Length <= 0 || items == null)
			{
				throw new ArgumentNullException(nameof(items), "The items is empty or null.");
			}

			if (select <= 0)
			{
				throw new ArgumentException(nameof(select), "The number of elements to be retrieved is negative or less than 1.");
			}
			else if (select > items.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(select), "The number of elements to be retrieved exceeds the items size.");
			}
			else if (select == items.Length)
			{
				return items;
			}

			T[] reservoir = new T[select];
			int index;

			Array.Copy(items, 0, reservoir, 0, reservoir.Length);

			for (var i = select; i < items.Length; i++)
			{
				index = (int)this.NextInt(0, (uint)i);

				if (index < select)
				{
					reservoir[index] = items[i];
				}
			}

			return reservoir;
		}

		/// <inheritdoc/>
		public virtual Task<T[]> SampleAsync<T>(T[] items, int select)
		{
			return this.SampleAsync(items, select, CancellationToken.None);
		}

		/// <inheritdoc/>
		public virtual Task<T[]> SampleAsync<T>(T[] items, int select, CancellationToken cancellationToken)
		{
			return Task.Factory.StartNew(() =>
			{
				return this.Sample(items, select);
			}, cancellationToken);
		}

		/// <inheritdoc/>
		public virtual T[] Sample<T>(IEnumerable<T> items, int select)
		{
			return this.Sample(items.ToArray(), select);
		}

		/// <inheritdoc/>
		public virtual Task<T[]> SampleAsync<T>(IEnumerable<T> items, int select)
		{
			return this.SampleAsync(items.ToArray(), select, CancellationToken.None);
		}

		/// <inheritdoc/>
		public virtual Task<T[]> SampleAsync<T>(IEnumerable<T> items, int select, CancellationToken cancellationToken)
		{
			return this.SampleAsync(items.ToArray(), select, cancellationToken);
		}

		/// <inheritdoc/>
		public virtual T[] Shuffle<T>(T[] items)
		{
			if (items.Length <= 0 || items == null)
			{
				throw new ArgumentNullException(nameof(items), "The items is empty or null.");
			}

			var newArray = new T[items.Length];
			Array.Copy(items, newArray, newArray.Length);

			T temp;

			for (var i = newArray.Length - 1; i > 1; i--)
			{
				var index = this.NextInt(0, (uint)i);
				temp = newArray[i];
				newArray[i] = newArray[index];
				newArray[index] = temp;
			}

			return newArray;
		}

		/// <inheritdoc/>
		public virtual Task<T[]> ShuffleAsync<T>(T[] items)
		{
			return this.ShuffleAsync(items, CancellationToken.None);
		}

		/// <inheritdoc/>
		public virtual Task<T[]> ShuffleAsync<T>(T[] items, CancellationToken cancellationToken)
		{
			return Task.Factory.StartNew(() =>
			{
				return this.Shuffle(items);
			}, cancellationToken);
		}

		/// <inheritdoc/>
		public virtual T[] Shuffle<T>(IEnumerable<T> items)
		{
			return this.Shuffle(items.ToArray());
		}

		/// <inheritdoc/>
		public virtual Task<T[]> ShuffleAsync<T>(IEnumerable<T> items)
		{
			return this.ShuffleAsync(items.ToArray(), CancellationToken.None);
		}

		/// <inheritdoc/>
		public virtual Task<T[]> ShuffleAsync<T>(IEnumerable<T> items, CancellationToken cancellationToken)
		{
			return this.ShuffleAsync(items.ToArray(), cancellationToken);
		}

		/// <inheritdoc/>
		public virtual void ShuffleInPlace<T>(T[] items)
		{
			if (items.Length <= 0 || items == null)
			{
				throw new ArgumentNullException(nameof(items), "The items is empty or null.");
			}

			T temp;

			for (var i = items.Length - 1; i > 1; i--)
			{
				var index = this.NextInt(0, (uint)i);
				temp = items[i];
				items[i] = items[index];
				items[index] = temp;
			}
		}

		/// <inheritdoc/>
		public virtual Task ShuffleInPlaceAsync<T>(T[] items)
		{
			return this.ShuffleInPlaceAsync(items, CancellationToken.None);
		}

		/// <inheritdoc/>
		public virtual Task ShuffleInPlaceAsync<T>(T[] items, CancellationToken cancellationToken)
		{
			return Task.Factory.StartNew(() =>
			{
				this.ShuffleInPlace(items);
			}, cancellationToken);
		}
	}
}