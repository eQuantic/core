using eQuantic.Core.Outcomes.Builder;
using eQuantic.Core.Outcomes.Results;

namespace eQuantic.Core.Outcomes
{
    public static class Outcome
    {
        public static BasicResultBuilder FromBasicResult()
        {
            return new BasicResultBuilder();
        }

        public static BasicResultBuilder FromBasicResult(bool success)
        {
            return success ? FromBasicResult().WithSuccess() : FromBasicResult().WithError();
        }

        public static ItemResultBuilder<TItem> FromItemResult<TItem>()
        {
            return new ItemResultBuilder<TItem>();
        }

        public static ItemResultBuilder<TItem> FromItemResult<TItem>(bool success)
        {
            return success ? FromItemResult<TItem>().WithSuccess() : FromItemResult<TItem>().WithError();
        }

        public static ListResultBuilder<TItem> FromListResult<TItem>()
        {
            return new ListResultBuilder<TItem>();
        }

        public static ListResultBuilder<TItem> FromListResult<TItem>(bool success)
        {
            return success ? FromListResult<TItem>().WithSuccess() : FromListResult<TItem>().WithError();
        }

        public static PagedListResultBuilder<TItem> FromPagedListResult<TItem>()
        {
            return new PagedListResultBuilder<TItem>();
        }

        public static PagedListResultBuilder<TItem> FromPagedListResult<TItem>(bool success)
        {
            return success ? FromPagedListResult<TItem>().WithSuccess() : FromPagedListResult<TItem>().WithError();
        }
    }
}