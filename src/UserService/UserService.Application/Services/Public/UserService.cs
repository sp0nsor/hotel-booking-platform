using AutoMapper;
using CSharpFunctionalExtensions;
using FluentValidation;
using Hangfire;
using UserService.Application.DTOs;
using UserService.Application.Interfaces.Internal;
using UserService.Application.Interfaces.Public;
using UserService.Application.Requests;
using UserService.Infrastructure.Data.Entities;
using UserService.Infrastructure.Data.Specifications;
using UserService.Infrastructure.Interfaces.Data;

namespace UserService.Application.Services.Public
{
    public class UserService : IUserService
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IAccessTokenService _accessTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IValidator<UpdateUserInfoRequest> _updateRequestValidator;
        private readonly IValidator<ConfirmUserRequest> _confirmRequestValidator;
        private readonly IValidator<LoginUserRequest> _loginRequestValidator;
        private readonly IValidator<RegisterUserRequest> _registerRequestValidator;
        private readonly IConfirmCodeService _confirmCodeService;
        private readonly IEmailService _emailService;
        private readonly ICacheService _cacheService;
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;

        public UserService(
            IRepository<UserEntity> userRepository,
            IPasswordService passwordService,
            IMapper mapper,
            ICacheService cacheService,
            IEmailService emailService,
            IConfirmCodeService confirmCodeService,
            IValidator<RegisterUserRequest> registerRequestValidator,
            IValidator<UpdateUserInfoRequest> updateRequestValidator,
            IValidator<ConfirmUserRequest> confirmRequestValidator,
            IValidator<LoginUserRequest> loginRequestValidator,
            IRefreshTokenService refreshTokenService,
            IAccessTokenService accessTokenService,
            IBackgroundJobClient backgroundJobClient)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _mapper = mapper;
            _cacheService = cacheService;
            _emailService = emailService;
            _confirmCodeService = confirmCodeService;
            _registerRequestValidator = registerRequestValidator;
            _updateRequestValidator = updateRequestValidator;
            _confirmRequestValidator = confirmRequestValidator;
            _loginRequestValidator = loginRequestValidator;
            _refreshTokenService = refreshTokenService;
            _accessTokenService = accessTokenService;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<Result<UserDto>> GetUserByIdAsync(
            Guid id, 
            CancellationToken cancellationToken)
        {
            var specification = new GetUserByIdSpecification(id);
            var existingUser = await _userRepository.GetSingleAsync(
                specification, 
                cancellationToken);

            if (existingUser is null)
                return Result.Failure<UserDto>("User not foud");

            return _mapper.Map<UserDto>(existingUser);
        }

        public async Task<Result> RegisterUserAsync(
            RegisterUserRequest registerUserRequest,
            CancellationToken cancellationToken)
        {
            var validationResult = await _registerRequestValidator
                .ValidateAsync(registerUserRequest);

            if (!validationResult.IsValid)
                return Result.Failure(string.Join("; ", validationResult.Errors
                    .Select(e => e.ErrorMessage)));

            var specification = new GetUserByEmailSpecification(registerUserRequest.Email);
            var existingUser = await _userRepository.GetSingleAsync(
                specification,
                cancellationToken);

            if (existingUser != null)
                return Result.Failure("The user with this email already exists");

            var passwordHash = _passwordService.Generate(registerUserRequest.Password);

            var userEntity = _mapper.Map<UserEntity>(registerUserRequest);

            userEntity.PasswordHash = passwordHash;

            var confirmCode = await _confirmCodeService.GenerateCodeAsync(
                userEntity.Id,
                cancellationToken);

            _backgroundJobClient.Enqueue(() =>
                _emailService.SendEmailAsync(
                    userEntity.Email,
                    "Confirm code",
                    confirmCode,
                    cancellationToken
                )
            );

            userEntity.IsActivated = false;

            await _userRepository.CreateAsync(
                    userEntity,
                    cancellationToken);

            return Result.Success();
        }

        public async Task<Result<LoginDto>> LoginUserAsync(
            LoginUserRequest loginUserRequest,
            CancellationToken cancellationToken)
        {
            var validationResult = await _loginRequestValidator
                .ValidateAsync(loginUserRequest);

            if (!validationResult.IsValid)
                return Result.Failure<LoginDto>(string.Join("; ", validationResult.Errors
                    .Select(e => e.ErrorMessage)));

            var specification = new GetUserByEmailSpecification(loginUserRequest.Email);
            var userEntity = await _userRepository.GetSingleAsync(
                specification,
                cancellationToken);

            if (userEntity is null)
                return Result.Failure<LoginDto>("This user dosen`t exist");

            var isPasswordValid = _passwordService.Verify(
                loginUserRequest.Password,
                userEntity.PasswordHash);

            if (!userEntity.IsActivated || !isPasswordValid)
                return Result.Failure<LoginDto>("Something went wrong");

            var refreshTokenValue = await _refreshTokenService.CreateResreshTokenAsync(
                userEntity.Id,
                cancellationToken);

            var accessTokenValue = await _accessTokenService.CreateAccessTokenAsync(
                userEntity,
                cancellationToken);

            return new LoginDto(refreshTokenValue, accessTokenValue);
        }

        public async Task<Result> ConfirmUserAsync(
            ConfirmUserRequest confirmUserRequest,
            CancellationToken cancellationToken)
        {
            var validationResult = await _confirmRequestValidator
                .ValidateAsync(confirmUserRequest);

            if (!validationResult.IsValid)
                return Result.Failure(string.Join("; ", validationResult.Errors
                    .Select(e => e.ErrorMessage)));

            var storedUserId = await _confirmCodeService.ConfirmCodeAsync(
                confirmUserRequest.ConfirmCode,
                cancellationToken);

            if (storedUserId == Guid.Empty)
                return Result.Failure("Invalid confirm code");

            var specification = new GetUserByIdSpecification(storedUserId);
            var userEntity = await _userRepository.GetSingleAsync(
                specification,
                cancellationToken);

            userEntity.IsActivated = true;

            await _userRepository.UpdateAsync(
                userEntity,
                cancellationToken);

            return Result.Success();
        }

        public async Task<Result<LoginDto>> RefreshUserTokenAsync(
            string refreshTokenValue,
            CancellationToken cancellationToken)
        {
            var refreshTokenEntity = await _refreshTokenService.GetTokenByValueAsync(
                refreshTokenValue,
                cancellationToken);

            if (refreshTokenEntity is null || refreshTokenEntity.Expires < DateTime.UtcNow)
                return Result.Failure<LoginDto>("You need to re-login");

            var specification = new GetUserByIdSpecification(refreshTokenEntity.UserId);
            var userEntity = await _userRepository.GetSingleAsync(
                specification,
                cancellationToken);

            if (userEntity is null)
                return Result.Failure<LoginDto>("User is null");

            var newAccessTokenValue = await _accessTokenService.CreateAccessTokenAsync(
                userEntity,
                cancellationToken);

            var newRefreshTokenValue = await _refreshTokenService.CreateResreshTokenAsync(
                userEntity.Id,
                cancellationToken);

            return Result.Success(new LoginDto(newRefreshTokenValue, newAccessTokenValue));
        }

        public async Task<Result> LogoutUserAsync(
            string jwtTokenId,
            string refreshTokenValue,
            CancellationToken cancellationToken)
        {
            await _refreshTokenService.DeleteTokenAsync(
                refreshTokenValue,
                cancellationToken);

            await _accessTokenService.DeleteTokenAsync(
                jwtTokenId,
                cancellationToken);

            return Result.Success();
        }

        public async Task<Result> UpdateUserInfoAsync(
            Guid userId,
            UpdateUserInfoRequest updateUserInfoRequest,
            CancellationToken cancellationToken)
        {
            var validationResult = await _updateRequestValidator
                .ValidateAsync(updateUserInfoRequest);

            if (!validationResult.IsValid)
                return Result.Failure(string.Join("; ", validationResult.Errors
                    .Select(e => e.ErrorMessage)));

            var specification = new GetUserByIdSpecification(userId);
            var userEntity = await _userRepository.GetSingleAsync(
                specification,
                cancellationToken);

            if (userEntity is null)
                return Result.Failure("User not found");

            if (userEntity.Id != userId)
                return Result.Failure("Invalid operation");

            userEntity.FirstName = updateUserInfoRequest.FirstName;
            userEntity.LastName = updateUserInfoRequest.LastName;
            userEntity.PhoneNumber = updateUserInfoRequest.PhoneNumber;

            await _userRepository.UpdateAsync(
                userEntity,
                cancellationToken);

            return Result.Success();
        }
    }
}
