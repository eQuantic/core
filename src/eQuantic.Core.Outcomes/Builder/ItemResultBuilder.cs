using System;
using System.Collections.Generic;
using System.Linq;
using eQuantic.Core.Outcomes.Results;

namespace eQuantic.Core.Outcomes.Builder {
    public class ItemResultBuilder<TItem> : ResultBuilder<ItemResult<TItem>>
    {
        public ItemResultBuilder() : base(new ItemResult<TItem>()) {
		}

        public ItemResultBuilder<TItem> WithSuccess () {
			this.result.Success = true;
			return this;
		}

		public ItemResultBuilder<TItem> WithError () {
			this.result.Success = false;
			return this;
		}

		/// <summary>
        /// Add a formatted string to the end of the outcome's message collection.
        /// </summary>
        /// <param name="message">String with format pattern to add. The format patterns will be used in string.Format.</param>
        /// <param name="paramList">Shorthand for String.Format</param>
        /// <returns></returns>
        public ItemResultBuilder<TItem> WithMessageFormat(string message, params object[] paramList)
        {
            message = string.Format(message, paramList);

            this.result.Messages.Add(message);
            return this;
        }

        /// <summary>
		/// Add a string to the end of the outcome's message collection.
		/// </summary>
		/// <param name="message">String to add.</param>
		/// <returns></returns>
		public ItemResultBuilder<TItem> WithMessage(string message)
	    {
			this.result.Messages.Add(message);
		    return this;
	    }

        /// <summary>
        /// Append a list of strings to the end of the outcome's message collection.
        /// </summary>
        /// <param name="messages">Enum of strings to add.</param>
        /// <returns></returns>
        public ItemResultBuilder<TItem> WithMessage(IEnumerable<string> messages)
        {
            if (messages == null)
                return this;

            this.result.Messages.AddRange(messages);
            return this;
        }

        /// <summary>
        /// Adds messages from the specified outcome, if any.
        /// </summary>
        /// <param name="outcome">Source outcome that messages are pulled from.</param>
        public ItemResultBuilder<TItem> WithMessagesFrom(BasicResult result)
        {
            WithMessage(this.result.Messages);
            return this;
        }

        /// <summary>
        /// Alternate syntax for WithMessage. Adds a collection of messages to the end of the outcome's message list. 
        /// </summary>
        public ItemResultBuilder<TItem> WithMessagesFrom(IEnumerable<string> messages)
        {
            WithMessage(messages);
            return this;
        }

        public ItemResultBuilder<TItem> WithException(Exception ex)
        {
            this.result.Success = false;
            this.result.Status = ResultStatus.Error;

            DeepException(ex);

            return this;
        }

        public ItemResultBuilder<TItem> WithStatus(ResultStatus status)
	    {
			this.result.Status = status;
		    return this;
	    }

		public ItemResultBuilder<TItem> MergeWith(ItemResult<TItem> result)
        {
            this.result.Status = result.Status;
            this.result.Success = result.Success;
            this.result.Message = result.Message;
            this.result.Messages.AddRange(result.Messages);
            this.result.Item = result.Item;

            return this;
        }

        public ItemResultBuilder<TItem> WithItem(TItem item)
        {
            this.result.Item = item;
            return this;
        }
    }
}