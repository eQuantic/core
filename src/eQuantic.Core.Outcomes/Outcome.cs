using eQuantic.Core.Outcomes.Builder;
using eQuantic.Core.Outcomes.Results;

namespace eQuantic.Core.Outcomes
{
    public static class Outcome
    {
        public static BasicResultBuilder WithBasicResult()
        {
            return new BasicResultBuilder(new BasicResult());
        }

        public static BasicResultBuilder WithBasicResult(bool success)
        {
            return success ? WithBasicResult().WithSuccess() : WithBasicResult().WithError();
        }

        public static ItemResultBuilder<TItem> WithItemResult<TItem>()
        {
            return new ItemResultBuilder<TItem>(new ItemResult<TItem>());
        }

        public static ItemResultBuilder<TItem> WithItemResult<TItem>(bool success)
        {
            return success ? WithItemResult<TItem>().WithSuccess() : WithItemResult<TItem>().WithError();
        }

        public static ListResultBuilder<TItem> WithListResult<TItem>()
        {
            return new ListResultBuilder<TItem>(new ListResult<TItem>());
        }

        public static ListResultBuilder<TItem> WithListResult<TItem>(bool success)
        {
            return success ? WithListResult<TItem>().WithSuccess() : WithListResult<TItem>().WithError();
        }

        public static PagedListResultBuilder<TItem> WithPagedListResult<TItem>()
        {
            return new PagedListResultBuilder<TItem>(new PagedListResult<TItem>());
        }

        public static PagedListResultBuilder<TItem> WithPagedListResult<TItem>(bool success)
        {
            return success ? WithPagedListResult<TItem>().WithSuccess() : WithPagedListResult<TItem>().WithError();
        }
    }
}