using System;
using System.Collections.Generic;
using System.Linq;
using eQuantic.Core.Outcomes.Results;

namespace eQuantic.Core.Outcomes.Builder {
    public abstract class ResultBuilder<TResult>
        where TResult : BasicResult
    {
        protected readonly TResult result;
		protected ResultBuilder (TResult result) {
			this.result = result;

		}

        public TResult Result () {
			return this.result;
		}

        protected void DeepException(Exception ex)
        {
            this.result.Messages.Add(ex.Message);
            if(ex.InnerException != null)
                DeepException(ex.InnerException);
        }
    }
}