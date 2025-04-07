namespace UserService.Application.Interfaces.Internal
{
    public interface IConfirmCodeService
    {
        Task<Guid> ConfirmCodeAsync(string code, CancellationToken cancellationToken);
        Task<string> GenerateCodeAsync(Guid userId, CancellationToken cancellationToken);
    }
}
