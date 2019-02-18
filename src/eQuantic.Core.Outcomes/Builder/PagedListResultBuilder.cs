using System;
using System.Collections.Generic;
using System.Linq;
using eQuantic.Core.Collections;
using eQuantic.Core.Outcomes.Results;

namespace eQuantic.Core.Outcomes.Builder {
    public class PagedListResultBuilder<TItem> : ResultBuilder<PagedListResult<TItem>>
    {
        public PagedListResultBuilder() : base(new PagedListResult<TItem>()) {
		}

        public PagedListResultBuilder<TItem> WithSuccess () {
			this.result.Success = true;
			return this;
		}

		public PagedListResultBuilder<TItem> WithError () {
			this.result.Success = false;
			return this;
		}

		/// <summary>
        /// Add a formatted string to the end of the outcome's message collection.
        /// </summary>
        /// <param name="message">String with format pattern to add. The format patterns will be used in string.Format.</param>
        /// <param name="paramList">Shorthand for String.Format</param>
        /// <returns></returns>
        public PagedListResultBuilder<TItem> WithMessageFormat(string message, params object[] paramList)
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
		public PagedListResultBuilder<TItem> WithMessage(string message)
	    {
			this.result.Messages.Add(message);
		    return this;
	    }

        /// <summary>
        /// Append a list of strings to the end of the outcome's message collection.
        /// </summary>
        /// <param name="messages">Enum of strings to add.</param>
        /// <returns></returns>
        public PagedListResultBuilder<TItem> WithMessage(IEnumerable<string> messages)
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
        public PagedListResultBuilder<TItem> WithMessagesFrom(BasicResult result)
        {
            WithMessage(this.result.Messages);
            return this;
        }

        /// <summary>
        /// Alternate syntax for WithMessage. Adds a collection of messages to the end of the outcome's message list. 
        /// </summary>
        public PagedListResultBuilder<TItem> WithMessagesFrom(IEnumerable<string> messages)
        {
            WithMessage(messages);
            return this;
        }

        public PagedListResultBuilder<TItem> WithException(Exception ex)
        {
            this.result.Success = false;
            this.result.Status = ResultStatus.Error;

            DeepException(ex);

            return this;
        }

        public PagedListResultBuilder<TItem> WithStatus(ResultStatus status)
	    {
			this.result.Status = status;
		    return this;
	    }

		public PagedListResultBuilder<TItem> MergeWith(PagedListResult<TItem> result)
        {
            this.result.Status = result.Status;
            this.result.Success = result.Success;
            this.result.Message = result.Message;
            this.result.Messages.AddRange(result.Messages);
            this.result.Items = result.Items;
            this.result.PageIndex = result.PageIndex;
            this.result.PageSize = result.PageSize;
            this.result.TotalCount = result.TotalCount;
            return this;
        }

        public PagedListResultBuilder<TItem> WithPagedItems(IPagedEnumerable<TItem> items)
        {
            this.result.Items = items.ToList();
            this.result.PageIndex = items.PageIndex;
            this.result.PageSize = items.PageSize;
            this.result.TotalCount = items.TotalCount;

            return this;
        }
    }
}