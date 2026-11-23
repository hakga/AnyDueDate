namespace App.Domain.Entities.Applications {
    /// <summary>応用データと、その最新（未変更・未破棄）の期限・対象者の組。</summary>
    public sealed class CurrentApplication<TApplication> where TApplication : DueDateApplication {
        public required TApplication Application { get; init; }
        public required DueDateUnit Current { get; init; }
        public required TargetUnit Target { get; init; }
    }
}
