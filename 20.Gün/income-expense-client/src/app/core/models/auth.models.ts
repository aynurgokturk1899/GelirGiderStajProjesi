export interface LoginRequest { email: string; password: string; }
export interface RegisterRequest { firstName: string; lastName: string; email: string; password: string; confirmPassword: string; }
export interface AuthenticatedUser { id: number; firstName: string; lastName: string; email: string; }
export interface AuthResponse { accessToken: string; expiresAtUtc: string; user: AuthenticatedUser; }
export interface ApiProblemDetails { title?: string; detail?: string; errors?: Record<string, string[]>; }
