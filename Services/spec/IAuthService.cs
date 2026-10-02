using System;
using EBlumbit.Dto.Auth;

namespace EBlumbit.Services.spec;

public interface IAuthService
{
    Task<AuthResponse> Login(AuthRequest request);

    Task<AuthResponse> RefreshToken(RefreshTokenRequest request);
}
