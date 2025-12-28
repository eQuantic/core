namespace eQuantic.Core.Eventing.Outbox;

/// <summary>
/// Interface for storing and retrieving outbox messages.
/// </summary>
public interface IOutboxStore
{
    /// <summary>
    /// Adds a message to the outbox.
    /// </summary>
    /// <param name="message">The message to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task AddAsync(IOutboxMessage message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds multiple messages to the outbox.
    /// </summary>
    /// <param name="messages">The messages to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task AddRangeAsync(IEnumerable<IOutboxMessage> messages, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets pending messages that haven't been processed.
    /// </summary>
    /// <param name="batchSize">Maximum number of messages to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IReadOnlyList<IOutboxMessage>> GetPendingAsync(int batchSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks a message as processed.
    /// </summary>
    /// <param name="id">The message ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task MarkAsProcessedAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks a message as failed with an error.
    /// </summary>
    /// <param name="id">The message ID.</param>
    /// <param name="error">The error message.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task MarkAsFailedAsync(Guid id, string error, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes processed messages older than the specified time.
    /// </summary>
    /// <param name="olderThan">Delete messages processed before this time.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task DeleteProcessedAsync(DateTimeOffset olderThan, CancellationToken cancellationToken = default);
}
