using Application.Contracts.Authentication;
using Application.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands.Login
{
    internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, OneOf<LoginDto, Error>>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRefreshRepository _refreshRepository;
        private readonly IMapper _mapper;
        private readonly ITokenProvider _tokenProvider;
        public LoginCommandHandler(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IRefreshRepository refreshRepository,
            IMapper mapper,
            ITokenProvider tokenProvider
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _refreshRepository = refreshRepository;
            _mapper = mapper;
            _tokenProvider = tokenProvider;
        }

        public async Task<OneOf<LoginDto, Error>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return UserErrors.InvalidCredentials;
            }
            var signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
            if (signInResult.Succeeded)
            {
                _tokenProvider.GenerateJwtToken(user!); 
                var refreshToken = _tokenProvider.GenerateRefreshToken();
                _refreshRepository.AddRefreshToken(user, refreshToken.token, refreshToken.expiresOn, cancellationToken);
                await _refreshRepository.SaveAsync(cancellationToken);
                return _mapper.Map<LoginDto>(user);
            }
            if (signInResult.IsNotAllowed)
            {
                return UserErrors.EmailNotConfirmed;
            }
            return UserErrors.InvalidCredentials;
        }
    }
}
