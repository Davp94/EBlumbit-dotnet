namespace EBlumbit.Dto.Auth;

public record AuthRequest
(
    string? Email,
    string? Username,
    string Password
);
